using Microsoft.Extensions.DependencyInjection;
using TPSocios.Forms;
using TPSocios.Repositorios;

namespace TPSocios
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Contains("--pruebas"))
            {
                Environment.ExitCode = Pruebas.Ejecutar();
                return;
            }

            ServiceCollection services = new ServiceCollection();

            services.AddScoped<ISocioRepository, SocioRepositorySQL>();

            services.AddTransient<frmTPSocios>();

            using ServiceProvider provider = services.BuildServiceProvider();

            ApplicationConfiguration.Initialize();
            Application.Run(provider.GetRequiredService<frmTPSocios>());
        }
    }
}
