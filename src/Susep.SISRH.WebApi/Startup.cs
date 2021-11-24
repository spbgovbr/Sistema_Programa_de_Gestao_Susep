using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Susep.SISRH.Application.Configurations;
using SUSEP.Framework.CoreFilters.Concrete;
using System;
using System.IO;

namespace Susep.SISRH.WebApi
{
    /// <summary>
    /// Classe que faz a configuração inicial da aplicação
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Parametros de configuraçao do sistema
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Construtor das configuraçoes do sistema
        /// </summary>
        public IConfigurationBuilder Builder { get; }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="env"></param>
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configura os serviços do sistema
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddAutofac();

            services.AddApiVersioning(p =>
            {
                p.DefaultApiVersion = new ApiVersion(1, 0);
                p.ReportApiVersions = true;
                p.AssumeDefaultVersionWhenUnspecified = true;
            });

            ContainerBuilder container = new ContainerBuilder();
            services.AddMvc(options =>
            {
                options.Filters.Add(new HeaderFilter());
                options.Filters.Add(new ProtocolFilter());
                //options.Filters.Add(new ValidatorFilter());
            });

            services.ConfigureOptions(Configuration);

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential() //AddSigningCredential()
                .AddInMemoryClients(IdentityServerConfiguration.GetClients())
                .AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources())
                .AddInMemoryApiScopes(IdentityServerConfiguration.GetApiScopes())
                .AddInMemoryApiResources(IdentityServerConfiguration.GetApiResources())
                .AddResourceOwnerValidator<Application.Auth.ResourceOwnerPasswordValidator>();
                //.AddLdapUsers<OpenLdapAppUser>(Configuration.GetSection("ldapActiveDirectory"), UserStore.InMemory);

            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Sistema de recursos humanos",
                        Version = "v1",
                        Description = "API REST para manutenção de dados de colaboradores da Susep",
                    });

                string caminhoAplicacao =
                    PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao =
                    PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc =
                    Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");

                c.IncludeXmlComments(caminhoXmlDoc);
            });


            services.ConfigureOptions(Configuration);            

            container.Populate(services);
            container.RegisterModule(new MediatorModuleConfiguration());
            container.RegisterModule(new ApplicationModuleConfiguration(Configuration));

            return new AutofacServiceProvider(container.Build());
        }

        /// <summary>
        /// Configura o sistema
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="log"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Homolog"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(option => option.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader());

            app.UseRouting();
            app.UseIdentityServer();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Sistema de corretores");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
