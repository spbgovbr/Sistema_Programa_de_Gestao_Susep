using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Logging.EventLog;
using Serilog;
using Serilog.Events;

namespace Susep.SISRH.WebApi
{
    /// <summary>
    /// Classe de execução do programa
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Constrói o programa inicial
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Cria o Host Web
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var environmentName = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Path.Combine(System.Environment.CurrentDirectory, "Settings"))
                                        .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                                        .AddEnvironmentVariables();

            return WebHost.CreateDefaultBuilder(args)
                   .ConfigureLogging(logging =>
                   {
                       logging.ClearProviders();
                    //    logging.AddEventLog(new EventLogSettings() { Filter = (source, level) => level == LogLevel.Error });
                   })
                   .UseStartup<Startup>()
                   .UseConfiguration(configurationBuilder.Build())
                   .UseSerilog((context, config) => {
                        config.ReadFrom.Configuration(context.Configuration);

                        // config
                        //     .Enrich.FromLogContext()
                        //     .WriteTo.File(@"Logs\log.txt", rollingInterval: RollingInterval.Day);
                    });
        }
    }
}
