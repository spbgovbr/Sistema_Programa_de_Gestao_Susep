using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Susep.SISRH.ApiGateway.Configurations
{
    public static class IdentityServerConfiguration
    {
        public static void ConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            {
                JwtBearerOptions opt = configuration.GetSection("identityServer:jwtBearerOptions").Get<JwtBearerOptions>();

                options.Authority = opt.Authority;
                options.Audience = opt.Audience;
                options.RequireHttpsMetadata = opt.RequireHttpsMetadata;
            });
        }
    }
}
