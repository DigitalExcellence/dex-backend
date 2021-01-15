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

using API.Configuration;
using API.InternalResources;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Models;
using Models.Defaults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Services.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace API.Extensions
{
    internal static class UsersExtensions
    {
        /// <summary>
        /// Gets the identity identifier.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <param name="actionContext">The action context.</param>
        /// <returns>The users identity id as string</returns>
        /// <exception cref="UnauthorizedAccessException">
        /// User is not authenticated!
        /// or
        /// The back-end header isn't added!
        /// </exception>
        /// <exception cref="NotSupportedException">The jwt doesn't have a sub</exception>
        public static string GetIdentityId(this ClaimsPrincipal claimsPrincipal, HttpContext actionContext)
        {
            string identityId;

            if(claimsPrincipal.Identities.Any(i => !i.IsAuthenticated))
            {
                throw new UnauthorizedAccessException("User is not authenticated!");
            }

            if(claimsPrincipal.IsInRole(Defaults.Roles.BackendApplication) || claimsPrincipal.HasClaim("client_role", Defaults.Roles.BackendApplication))
            {
                string identityIdHeader = actionContext.Request.Headers.SingleOrDefault(h => h.Key == "IdentityId")
                                                      .Value
                                                      .FirstOrDefault();

                if(string.IsNullOrWhiteSpace(identityIdHeader))
                {
                    throw new UnauthorizedAccessException("The back-end header isn't added!");
                }

                identityId = identityIdHeader;
            } else
            {
                string sub = claimsPrincipal.Claims.FirstOrDefault(c => c.Type.Equals("sub"))
                                            ?.Value;
                if(sub == null)
                {
                    throw new NotSupportedException("The jwt doesn't have a subject identifier.");
                }

                return sub;
            }

            return identityId;
        }

        /// <summary>
        /// Gets the context user.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <param name="userService">The user service.</param>
        /// <returns>The current users user object.</returns>
        public static async Task<User> GetContextUser(this HttpContext actionContext, IUserService userService)
        {
            string identityProverId = actionContext.User.GetIdentityId(actionContext);
            return await userService.GetUserByIdentityIdAsync(identityProverId);
        }


        /// <summary>
        /// Gets the user information synchronous.
        /// this is triggered when a user makes a request who does not have an account already.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>The user object with information retrieved from the identity server</returns>
        public static UserCreateInternalResource GetUserInformation(this HttpContext actionContext, Config config)
        {
            string bearerToken = actionContext.Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
            string providerId = "";

            if(bearerToken != null)
            {
                string token = bearerToken.Replace("Bearer ", "");
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                if(handler.ReadToken(token) is JwtSecurityToken tokens)
                {
                    providerId = tokens.Claims.FirstOrDefault(claim => claim.Type == "idp")?.Value;
                }
            }

            if(string.IsNullOrEmpty(bearerToken))
            {
                return null;
            }
            // Not sure maybe has to be retrieved from the originating identity server aka from the token iss.
            RestClient client = new RestClient(config.IdentityServer.IdentityUrl + "/connect/userinfo");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", bearerToken);
            IRestResponse response = client.Execute(request);
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            if(jsonResponse?.ContainsKey("name") != true ||
               jsonResponse?.ContainsKey("email") != true ||
               jsonResponse?.ContainsKey("sub") != true)
            {
                return null;
            }

            UserCreateInternalResource newUser = new UserCreateInternalResource
            {
                Name = (string) jsonResponse["name"],
                Email = (string) jsonResponse["email"],
                IdentityId = (string) jsonResponse["sub"],
                IdentityInstitutionId = providerId
            };
            return newUser ;
        }

        public static bool CheckIfUserIsAuthenticated(this ClaimsPrincipal claimsPrincipal)
        {
            if(claimsPrincipal.Identities.Any(i => i.IsAuthenticated))
            {
                return true;
            }

            return false;
        }

    }
}
