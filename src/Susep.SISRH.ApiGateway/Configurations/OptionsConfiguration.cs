using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Susep.SISRH.ApiGateway.Configurations
{
    public static class OptionsConfiguration
    {
        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtBearerOptions>(options => configuration.GetSection("jwtBearerOptions").Bind(options));
        }
    }
}
