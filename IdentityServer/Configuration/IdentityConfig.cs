﻿/*
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

        public static IEnumerable<Client> Clients(Config config) =>
            new Client[]
            {
                // machine to machine client (Identity -> API)
                new Client
                {
                    ClientId = "dex-api-client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret(config.Self.IdentityApplications.Single(a => a["Key"].Equals("dex-api"))["Value"].Sha256())
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
                    ClientSecrets = { new Secret(config.Self.IdentityApplications.Single(a => a["Key"].Equals("dex-frontend"))["Value"].Sha256()) },

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