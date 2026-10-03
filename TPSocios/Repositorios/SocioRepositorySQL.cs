using System.Data;
using Microsoft.Data.SqlClient;
using TPSocios.Datos;
using TPSocios.Entidades;

namespace TPSocios.Repositorios
{
    public class SocioRepositorySQL : ISocioRepository
    {
        private readonly Database _database;

        public SocioRepositorySQL()
            : this(new Database())
        {
        }

        public SocioRepositorySQL(string cadenaConexion)
            : this(new Database(cadenaConexion))
        {
        }

        public SocioRepositorySQL(Database database)
        {
            ArgumentNullException.ThrowIfNull(database);

            this._database = database;
        }

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

        public async Task EliminarAsync(int idSocio, CancellationToken cancellationToken = default)
        {
            const string consulta =
                @"DELETE FROM dbo.Socios WHERE IdSocio = @IdSocio";

            await this._database.EjecutarAsync(consulta, cancellationToken,
                new SqlParameter("@IdSocio", SqlDbType.Int) { Value = idSocio });
        }

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
