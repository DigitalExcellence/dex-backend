/*
* Digital Excellence Copyright (C) 2020 Brend Smits
* 
* This program is free software: you can redistribute it and/or modify 
* it under the terms of the GNU Lesser General Public License as published 
* by the Free Software Foundation version 3 of the License.
* 
* This program is distributed in the hope that it will be useful, 
* but WITHOUT ANY WARRANTY; without even the implied warranty 
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
* See the GNU Lesser General Public License for more details.
* 
* You can find a copy of the GNU Lesser General Public License 
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using Configuration;
using IdentityServer4;
using IdentityServer4.Models;
using Models.Defaults;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Configuration
{

    public static class IdentityConfig
    {

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
        
        public static IEnumerable<ApiResource> Apis =>
            new[]
            {
                new ApiResource("dex-api", "Digital Excellence API")
                {
                    Scopes =
                    {
                        new Scope(nameof(Defaults.Scopes.ProjectRead)),
                        new Scope(nameof(Defaults.Scopes.ProjectWrite)),
                        new Scope(nameof(Defaults.Scopes.UserWrite)),
                        new Scope(nameof(Defaults.Scopes.UserRead)),
                        new Scope(nameof(Defaults.Scopes.HighlightWrite)),
                        new Scope(nameof(Defaults.Scopes.HighlightRead))
                    }
                }
            };

        public static IEnumerable<Client> Clients(Config config)
        {
            return new[]
                   {
                       // machine to machine client (Identity -> API)
                       new Client
                       {
                           ClientId = "dex-api-client",
                           AllowedGrantTypes = GrantTypes.ClientCredentials,
                           ClientSecrets =
                           {
                               new Secret(config.Self.IdentityApplications.Single(a => a["Key"]
                                                                                      .Equals("dex-api"))["Value"]
                                                .Sha256())
                           },
                           AllowedScopes =
                           {
                               nameof(Defaults.Scopes.ProjectRead),
                               nameof(Defaults.Scopes.ProjectWrite),
                               nameof(Defaults.Scopes.UserWrite),
                               nameof(Defaults.Scopes.UserRead),
                               nameof(Defaults.Scopes.HighlightRead),
                               nameof(Defaults.Scopes.HighlightWrite)
                           }
                       },

                       // interactive ASP.NET Core MVC client
                       new Client
                       {
                           ClientId = "dex-frontend",
                           ClientName = "Digital Excellence Angular Frontend",
                           ClientSecrets =
                           {
                               new Secret(config.Self.IdentityApplications.Single(a => a["Key"]
                                                                                      .Equals("dex-frontend"))["Value"].Sha256())
                           },
                           AllowedGrantTypes = GrantTypes.Implicit,
                           RequirePkce = true,

                           // where to redirect to after login
                           RedirectUris = config.Frontend.RedirectUrisFrontend,

                           // where to redirect to after logout
                           PostLogoutRedirectUris = config.Frontend.PostLogoutUrisFrontend,

                           AllowedScopes = new List<string>
                            {
                                IdentityServerConstants.StandardScopes.OpenId,
                                IdentityServerConstants.StandardScopes.Profile,
                                IdentityServerConstants.StandardScopes.Email,
                                "dex-api"

                            },
                           AllowAccessTokensViaBrowser = true

                           // AllowOfflineAccess = true
                       },

                       new Client
                       {
                           ClientId = "Swagger-UI",
                           ClientName = "Swagger UI",
                           AllowedGrantTypes = GrantTypes.Implicit,
                           AllowAccessTokensViaBrowser = true,
                           AlwaysIncludeUserClaimsInIdToken = true,
                           RedirectUris = config.Swagger.RedirectUrisSwagger,
                           PostLogoutRedirectUris = config.Swagger.PostLogoutUrisSwagger,
                           AllowedScopes = new List<string>
                            {

                                IdentityServerConstants.StandardScopes.OpenId,
                                IdentityServerConstants.StandardScopes.Profile,
                                nameof(Defaults.Scopes.ProjectRead),
                                nameof(Defaults.Scopes.ProjectWrite),
                                nameof(Defaults.Scopes.UserWrite),
                                nameof(Defaults.Scopes.UserRead),
                                nameof(Defaults.Scopes.HighlightRead),
                                nameof(Defaults.Scopes.HighlightWrite)
                            },
                       }
                   };
        }

    }

}
