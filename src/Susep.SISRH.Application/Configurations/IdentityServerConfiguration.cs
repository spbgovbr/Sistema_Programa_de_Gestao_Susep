using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Susep.SISRH.Application.Configurations
{
    public static class IdentityServerConfiguration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                new ApiScope("SISGP.ProgramaGestao", "API do programa de gestão")
            };

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("SISGP.APIGateway", "API Gateway do programa de gestão")
                {
                    Scopes = { "SISGP.ProgramaGestao" }
                }
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {

                // OpenID Connect implicit flow client (MVC with vue js)
                new Client
                {
                    ClientId = "SISGP.Web",
                    ClientName = "Cliente web do SISGP",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "SISGP.ProgramaGestao"
                    },
                    AllowOfflineAccess = true
                }
            };
        }
    }
}
