using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Susep.SISRH.Application.Options;
using SUSEP.Framework.MessageBroker.Options;
using SUSEP.Framework.Utils.Options;

namespace Susep.SISRH.Application.Configurations
{
    public static class OptionsConfiguration
    {
        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<JwtBearerOptions>(options => configuration.GetSection("jwtBearerOptions").Bind(options));
            //services.Configure<BrokerOptions>(options => configuration.GetSection("brokerOptions").Bind(options));
            services.Configure<EmailOptions>(options => configuration.GetSection("emailOptions").Bind(options));
            services.Configure<LdapOptions>(options => configuration.GetSection("ldapOptions").Bind(options));

        }
    }
}
