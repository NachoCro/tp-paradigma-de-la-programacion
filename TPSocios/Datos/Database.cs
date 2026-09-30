using System.Data;
using Microsoft.Data.SqlClient;
using TPSocios.Configuracion;

namespace TPSocios.Datos
{
    /// <summary>
    /// Clase auxiliar que centraliza el acceso a SQL Server con ADO.NET.
    ///
    /// Es la única capa del proyecto que crea objetos <see cref="SqlConnection"/>,
    /// <see cref="SqlCommand"/> y <see cref="SqlParameter"/>. Por eso, el resto
    /// del código (por ejemplo <c>SocioRepositorySQL</c>) no repite la apertura
    /// y el cierre de conexiones.
    ///
    /// Dos decisiones de diseño importantes:
    ///
    /// 1. RECURSOS: todos los objetos se crean dentro de bloques
    ///    <c>await using</c>. Eso garantiza el cierre de la conexión incluso si
    ///    una consulta lanza una excepción, de modo que nunca quedan conexiones
    ///    colgadas ni se agotan las del grupo (pool).
    ///
    /// 2. INYECCIÓN DE SQL: el texto SQL se envía siempre por separado de los
    ///    datos, que viajan en objetos <see cref="SqlParameter"/>. Nunca se
    ///    concatena texto tipeado por el usuario dentro de la consulta, porque
    ///    eso permitiría que un valor malicioso se ejecute como si fuera código
    ///    de SQL.
    /// </summary>
    public class Database
    {
        private readonly string _cadenaConexion;

        /// <summary>
        /// Constructor que toma la cadena de conexión de App.config.
        /// </summary>
        public Database()
            : this(CadenaConexion.Valor)
        {
        }

        /// <summary>
        /// Constructor sobrecargado que permite indicar otra cadena de conexión,
        /// útil para pruebas o para apuntar a otra base de datos.
        /// </summary>
        public Database(string cadenaConexion)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cadenaConexion);

            this._cadenaConexion = cadenaConexion;
        }

        /// <summary>
        /// Ejecuta una consulta SELECT y devuelve los resultados como
        /// <see cref="DataTable"/>, que es el tipo que el <c>DataGridView</c>
        /// puede mostrar directamente asignándolo a su propiedad DataSource.
        /// </summary>
        public async Task<DataTable> ConsultarAsync(string sql, CancellationToken cancellationToken = default,
                                                     params SqlParameter[] parametros)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);

            await using SqlConnection conexion = await this.AbrirConexionAsync(cancellationToken);
            await using SqlCommand comando = new SqlCommand(sql, conexion);

            AgregarParametros(comando, parametros);

            DataTable tabla = new DataTable();

            using (SqlDataReader lector = await comando.ExecuteReaderAsync(cancellationToken))
            {
                tabla.Load(lector);
            }

            return tabla;
        }

        /// <summary>
        /// Ejecuta un comando INSERT, UPDATE o DELETE y devuelve la cantidad
        /// de filas afectadas.
        /// </summary>
        public async Task<int> EjecutarAsync(string sql, CancellationToken cancellationToken = default,
                                             params SqlParameter[] parametros)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);

            await using SqlConnection conexion = await this.AbrirConexionAsync(cancellationToken);
            await using SqlCommand comando = new SqlCommand(sql, conexion);

            AgregarParametros(comando, parametros);

            return await comando.ExecuteNonQueryAsync(cancellationToken);
        }

        /// <summary>
        /// Ejecuta una consulta que devuelve un único valor, como un COUNT o un
        /// SCOPE_IDENTITY. Se usa para los controles de la base de datos.
        /// </summary>
        public async Task<object?> EscalarAsync(string sql, CancellationToken cancellationToken = default,
                                                     params SqlParameter[] parametros)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);

            await using SqlConnection conexion = await this.AbrirConexionAsync(cancellationToken);
            await using SqlCommand comando = new SqlCommand(sql, conexion);

            AgregarParametros(comando, parametros);

            return await comando.ExecuteScalarAsync(cancellationToken);
        }

        /// <summary>
        /// Crea y abre una conexión con la cadena de conexión configurada.
        /// Se entrega ya abierta para que el bloque await using del método
        /// llamador la cierre al terminar.
        /// </summary>
        private async Task<SqlConnection> AbrirConexionAsync(CancellationToken cancellationToken)
        {
            SqlConnection conexion = new SqlConnection(this._cadenaConexion);

            try
            {
                await conexion.OpenAsync(cancellationToken);
                return conexion;
            }
            catch
            {
                // Si la apertura falla, la conexión queda en un estado
                // intermediario: hay que cerrarla a mano antes de propagar
                // el error, porque todavía no entró en un bloque using.
                await conexion.DisposeAsync();
                throw;
            }
        }

        /// <summary>
        /// Agrega los parámetros recibidos al comando.
        /// </summary>
        private static void AgregarParametros(SqlCommand comando, SqlParameter[] parametros)
        {
            if (parametros is null)
            {
                return;
            }

            foreach (SqlParameter parametro in parametros)
            {
                comando.Parameters.Add(parametro);
            }
        }
    }
}