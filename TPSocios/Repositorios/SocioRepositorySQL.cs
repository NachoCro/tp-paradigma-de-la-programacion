using System.Data;
using Microsoft.Data.SqlClient;
using TPSocios.Configuracion;
using TPSocios.Datos;
using TPSocios.Entidades;

namespace TPSocios.Repositorios
{
    /// <summary>
    /// Implementación funcional de ISocioRepository sobre SQL Server.
    ///
    /// Esta clase NO abre conexiones ni crea comandos: delega esa tarea en
    /// <see cref="Database"/>, que es la única que conoce el detalle de ADO.NET.
    /// Acá solamente se traduce de una tabla SQL a un objeto <see cref="Socio"/>
    /// y viceversa. De ese modo, el acceso a la base de datos está en un solo
    /// lugar y esta capa se ocupa únicamente del modelo.
    ///
    /// Todas las operaciones son asíncronas y se apoyan en comandos
    /// parametrizados para evitar la inyección de SQL.
    /// La responsabilidad de esta clase se limita a acceder a los datos:
    /// no valida reglas de negocio, porque eso le corresponde a la capa de UI.
    /// </summary>
    public class SocioRepositorySQL : ISocioRepository
    {
        private readonly Database _database;

        /// <summary>
        /// Constructor utilizado por el contenedor de inyección de dependencias.
        /// Toma la cadena de conexión desde la configuración del proyecto.
        /// </summary>
        public SocioRepositorySQL()
            : this(new Database(CadenaConexion.Valor))
        {
        }

        /// <summary>
        /// Constructor sobrecargado que permite indicar otra cadena de conexión.
        /// </summary>
        public SocioRepositorySQL(string cadenaConexion)
            : this(new Database(cadenaConexion))
        {
        }

        /// <summary>
        /// Constructor que recibe directamente el ayudante de base de datos.
        /// </summary>
        public SocioRepositorySQL(Database database)
        {
            ArgumentNullException.ThrowIfNull(database);

            this._database = database;
        }

        /// <summary>
        /// Lectura de todos los socios de la tabla.
        /// La consulta devuelve un DataTable, que se recorre para armar la
        /// lista de objetos Socio.
        /// </summary>
        public async Task<List<Socio>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            const string consulta =
                @"SELECT IdSocio, LegajoSocio, Apellido, Nombre, Email,
                         FechaNacimiento, CuotaMensual, TipoSocio, Activo
                  FROM dbo.Socios
                  ORDER BY Apellido, Nombre";

            DataTable tabla = await this._database.ConsultarAsync(consulta, cancellationToken);

            List<Socio> socios = new List<Socio>(tabla.Rows.Count);

            foreach (DataRow fila in tabla.Rows)
            {
                socios.Add(Construir(fila));
            }

            return socios;
        }

        /// <summary>
        /// Alta de un socio. SCOPE_IDENTITY devuelve el IdSocio generado
        /// por la columna IDENTITY de la tabla.
        /// </summary>
        public async Task<int> InsertarAsync(Socio socio, CancellationToken cancellationToken = default)
        {
            const string consulta =
                @"INSERT INTO dbo.Socios
                      (LegajoSocio, Apellido, Nombre, Email, FechaNacimiento, CuotaMensual, TipoSocio, Activo)
                  VALUES
                      (@LegajoSocio, @Apellido, @Nombre, @Email, @FechaNacimiento, @CuotaMensual, @TipoSocio, @Activo);
                  SELECT CAST(SCOPE_IDENTITY() AS int);";

            SqlParameter[] parametros = CrearParametros(socio);

            object? resultado = await this._database.EscalarAsync(consulta, cancellationToken, parametros);

            return resultado is null || resultado is DBNull
                ? 0
                : Convert.ToInt32(resultado);
        }

        /// <summary>
        /// Modificación del socio cuyo IdSocio coincide con el recibido.
        /// </summary>
        public async Task ActualizarAsync(Socio socio, CancellationToken cancellationToken = default)
        {
            const string consulta =
                @"UPDATE dbo.Socios
                  SET LegajoSocio     = @LegajoSocio,
                      Apellido        = @Apellido,
                      Nombre          = @Nombre,
                      Email           = @Email,
                      FechaNacimiento = @FechaNacimiento,
                      CuotaMensual    = @CuotaMensual,
                      TipoSocio       = @TipoSocio,
                      Activo          = @Activo
                  WHERE IdSocio = @IdSocio";

            List<SqlParameter> parametros = new List<SqlParameter>(CrearParametros(socio))
            {
                new SqlParameter("@IdSocio", SqlDbType.Int) { Value = socio.IdSocio }
            };

            await this._database.EjecutarAsync(consulta, cancellationToken, parametros.ToArray());
        }

        /// <summary>
        /// Baja del socio identificado por el IdSocio recibido.
        /// </summary>
        public async Task EliminarAsync(int idSocio, CancellationToken cancellationToken = default)
        {
            const string consulta =
                @"DELETE FROM dbo.Socios WHERE IdSocio = @IdSocio";

            await this._database.EjecutarAsync(consulta, cancellationToken,
                new SqlParameter("@IdSocio", SqlDbType.Int) { Value = idSocio });
        }

        /// <summary>
        /// Consulta de control de la unicidad del legajo.
        /// </summary>
        public async Task<bool> ExisteLegajoAsync(string legajoSocio, int idSocioExcluir = 0,
                                                 CancellationToken cancellationToken = default)
        {
            const string consulta =
                @"SELECT COUNT(1)
                  FROM dbo.Socios
                  WHERE LegajoSocio = @LegajoSocio
                    AND IdSocio <> @IdSocioExcluir";

            object? resultado = await this._database.EscalarAsync(consulta, cancellationToken,
                new SqlParameter("@LegajoSocio", SqlDbType.NVarChar, 6) { Value = legajoSocio },
                new SqlParameter("@IdSocioExcluir", SqlDbType.Int) { Value = idSocioExcluir });

            return resultado is not null && Convert.ToInt32(resultado) > 0;
        }

        /// <summary>
        /// Arma los parámetros que comparten el alta y la modificación.
        /// Los tipos se declaran explícitamente para que coincidan con los de
        /// la tabla. Al ser SqlParameter, ningún valor del usuario se interpola
        /// dentro del texto SQL.
        /// </summary>
        private static SqlParameter[] CrearParametros(Socio socio)
        {
            return new SqlParameter[]
            {
                new("@LegajoSocio", SqlDbType.NVarChar, 6) { Value = socio.LegajoSocio },
                new("@Apellido", SqlDbType.NVarChar, 50) { Value = socio.Apellido },
                new("@Nombre", SqlDbType.NVarChar, 50) { Value = socio.Nombre },
                new("@Email", SqlDbType.NVarChar, 100) { Value = socio.Email },
                new("@FechaNacimiento", SqlDbType.Date) { Value = socio.FechaNacimiento.Date },
                new("@CuotaMensual", SqlDbType.Decimal) { Value = socio.CuotaMensual },
                new("@TipoSocio", SqlDbType.NVarChar, 20) { Value = socio.TipoSocio.ToString() },
                new("@Activo", SqlDbType.Bit) { Value = socio.Disponible }
            };
        }

        /// <summary>
        /// Traduce una fila de la tabla resultado a un objeto Socio.
        /// </summary>
        private static Socio Construir(DataRow fila)
        {
            return new Socio(
                idSocio: Convert.ToInt32(fila["IdSocio"]),
                legajoSocio: fila["LegajoSocio"].ToString() ?? string.Empty,
                apellido: fila["Apellido"].ToString() ?? string.Empty,
                nombre: fila["Nombre"].ToString() ?? string.Empty,
                email: fila["Email"].ToString() ?? string.Empty,
                fechaNacimiento: Convert.ToDateTime(fila["FechaNacimiento"]),
                cuotaMensual: Convert.ToDecimal(fila["CuotaMensual"]),
                tipoSocio: Enum.TryParse(fila["TipoSocio"].ToString(), out TipoSocio tipoSocio)
                    ? tipoSocio
                    : TipoSocio.Mayor,
                disponible: Convert.ToBoolean(fila["Activo"]));
        }
    }
}