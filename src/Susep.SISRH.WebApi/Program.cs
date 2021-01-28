using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

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
                   .ConfigureLogging(logging =>
                   {
                       logging.ClearProviders();
                       logging.AddEventLog(new EventLogSettings() { Filter = (source, level) => level == LogLevel.Error });
                   })
                   .UseStartup<Startup>();
    }
}
