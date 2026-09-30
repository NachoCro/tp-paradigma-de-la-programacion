using System.Configuration;

namespace TPSocios.Configuracion
{
    /// <summary>
    /// Datos de acceso a la base de datos TPSocios.
    ///
    /// La cadena de conexión vive en App.config, dentro de la sección
    /// connectionStrings, con el nombre "MiConexion". Así se puede cambiar el
    /// servidor, el puerto o la contraseña editando el archivo de
    /// configuración, sin recompilar el proyecto.
    ///
    /// Si el archivo no está presente (por ejemplo, si se compiló en Linux o
    /// se copió solo el ejecutable), se usa el valor de respaldo
    /// <see cref="ValorPorDefecto"/> para que el proyecto siga funcionando.
    /// </summary>
    public static class CadenaConexion
    {
        /// <summary>
        /// Nombre de la entrada dentro de connectionStrings en App.config.
        /// </summary>
        public const string Nombre = "MiConexion";

        /// <summary>
        /// Valor de respaldo, equivalente al que figura en App.config.
        /// Se usa solamente si no se puede leer el archivo de configuración.
        ///
        /// TrustServerCertificate=True es indispensable en Docker, en LocalDB y
        /// en las instalaciones nativas: el certificado del servidor es
        /// autofirmado y, sin esta opción, la conexión falla con el error
        /// "certificate chain was issued by an authority that is not trusted".
        /// </summary>
        public const string ValorPorDefecto =
            @"Server=localhost,1433;Database=TPSocios;User Id=sa;Password=Password123!;TrustServerCertificate=True;";

        /// <summary>
        /// Cadena de conexión que usa la aplicación.
        /// Se resuelve una sola vez y se guarda en memoria, porque el archivo
        /// de configuración no cambia mientras la aplicación está abierta.
        /// </summary>
        public static string Valor { get; } = Leer();

        /// <summary>
        /// Lee la cadena de conexión desde App.config y, si no está disponible,
        /// devuelve el valor de respaldo.
        /// </summary>
        private static string Leer()
        {
            try
            {
                ConnectionStringSettings? configuracion =
                    ConfigurationManager.ConnectionStrings[Nombre];

                if (configuracion is not null &&
                    !string.IsNullOrWhiteSpace(configuracion.ConnectionString))
                {
                    return configuracion.ConnectionString;
                }
            }
            catch (ConfigurationErrorsException)
            {
                // El archivo no existe o tiene XML inválido: se informa abajo
                // mediante el valor de respaldo.
            }

            return ValorPorDefecto;
        }
    }
}