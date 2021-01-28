using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Susep.SISRH.ApiGateway
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var environmentName = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Path.Combine(System.Environment.CurrentDirectory, "Settings"))
                                        .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                                        .AddEnvironmentVariables();

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseConfiguration(configurationBuilder.Build())
                .UseSerilog((context, config) => {
                    config.ReadFrom.Configuration(context.Configuration);

                    config
                        .Enrich.FromLogContext()
                        .WriteTo.File(@"Logs\log.txt", rollingInterval: RollingInterval.Day);
                });
        }
    }
}
