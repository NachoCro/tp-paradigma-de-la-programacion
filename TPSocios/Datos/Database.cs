using System.Data;
using Microsoft.Data.SqlClient;
using TPSocios.Configuracion;

namespace TPSocios.Datos
{
    public class Database
    {
        private readonly string _cadenaConexion;

        public Database()
            : this(CadenaConexion.Valor)
        {
        }

        public Database(string cadenaConexion)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cadenaConexion);

            this._cadenaConexion = cadenaConexion;
        }

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

        public async Task<int> EjecutarAsync(string sql, CancellationToken cancellationToken = default,
                                             params SqlParameter[] parametros)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);

            await using SqlConnection conexion = await this.AbrirConexionAsync(cancellationToken);
            await using SqlCommand comando = new SqlCommand(sql, conexion);

            AgregarParametros(comando, parametros);

            return await comando.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task<object?> EscalarAsync(string sql, CancellationToken cancellationToken = default,
                                                     params SqlParameter[] parametros)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);

            await using SqlConnection conexion = await this.AbrirConexionAsync(cancellationToken);
            await using SqlCommand comando = new SqlCommand(sql, conexion);

            AgregarParametros(comando, parametros);

            return await comando.ExecuteScalarAsync(cancellationToken);
        }

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
                await conexion.DisposeAsync();
                throw;
            }
        }

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
