using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LAMBusiness.Escritorio
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            IConfiguration configuration;
            configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            var configuracion = configuration.GetSection("Configuracion").Get<Configuracion>();

            IServiceCollection services = new ServiceCollection();
            //services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<DataContext, DataContext>();
            services.AddTransient<Configuracion, Configuracion>();
            services.AddScoped<IniciarSesionForm>();

            Application.Run(new IniciarSesionForm(configuracion!));
        }
    }
}