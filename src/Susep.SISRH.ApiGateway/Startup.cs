using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Susep.SISRH.ApiGateway.Configurations;
using Susep.SISRH.ApiGateway.Middlewares;
using System.IO;


namespace Susep.SISRH.ApiGateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IConfigurationBuilder Builder { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.ConfigureCors();
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new CorsAuthorizationFilterFactory("CorsPolicy"));
            //});

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddMvc();
            services.ConfigureOptions(Configuration);
            services.ConfigureIdentityServer(Configuration);
            services.AddOcelot(Configuration).AddCacheManager(it => it.WithDictionaryHandle());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Homolog"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseOcelot().Wait();
        }
    }
}
