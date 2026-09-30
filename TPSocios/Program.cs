using Microsoft.Extensions.DependencyInjection;
using TPSocios.Forms;
using TPSocios.Repositorios;

namespace TPSocios
{
    /// <summary>
    /// Punto de entrada de la aplicación.
    /// Arma el contenedor de inyección de dependencias y muestra el formulario.
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Verificaciones rápidas de la TP que no necesitan la base de datos.
            if (args.Contains("--pruebas"))
            {
                Environment.ExitCode = Pruebas.Ejecutar();
                return;
            }

            ServiceCollection services = new ServiceCollection();

            // Se registra la interfaz y la clase concreta que la implementa,
            // para que el formulario trabaje con la abstracción y no con
            // la implementación. Para persistir sobre un archivo CSV en lugar
            // de SQL Server, alcanza con registrar SocioRepositoryCSV:
            // el resto del código no se modifica.
            services.AddScoped<ISocioRepository, SocioRepositorySQL>();

            // El formulario recibe el repositorio por el constructor.
            services.AddTransient<frmTPSocios>();

            using ServiceProvider provider = services.BuildServiceProvider();

            ApplicationConfiguration.Initialize();
            Application.Run(provider.GetRequiredService<frmTPSocios>());
        }
    }
}
