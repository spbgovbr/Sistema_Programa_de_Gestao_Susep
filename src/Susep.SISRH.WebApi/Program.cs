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
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration((hostingContext, config) =>
                       config.SetBasePath(Path.Combine(hostingContext.HostingEnvironment.ContentRootPath, "Settings"))
                             .AddJsonFile($"connectionstrings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                             .AddJsonFile($"messagebroker.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                             .AddEnvironmentVariables())
                    .UseSerilog((context, logger) => logger.ReadFrom.Configuration(context.Configuration))
                   .UseStartup<Startup>();

    }
}
