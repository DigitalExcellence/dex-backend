using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;
using Models.Defaults;

namespace IdentityServer.Configuration
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("dex-api", "Digital Excellence API")
                {
                    Scopes =
                    {
                        new Scope(nameof(Defaults.ScopeCategories.ApiDataRead)),
                        new Scope(nameof(Defaults.ScopeCategories.ApiDataWrite))
                    }
                }, 
            };
        
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // machine to machine client (Identity -> API)
                new Client
                {
                    ClientId = "dex-api-client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        nameof(Defaults.ScopeCategories.ApiDataRead),
                        nameof(Defaults.ScopeCategories.ApiDataWrite)
                    }
                },
                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "dex-frontend",
                    ClientName = "Digital Excellence Angular Frontend",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        nameof(Defaults.ScopeCategories.ApiDataRead)
                    }
                    // AllowOfflineAccess = true
                }
            };
        
    }
}