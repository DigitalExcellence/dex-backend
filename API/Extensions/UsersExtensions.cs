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

namespace API.Extensions
{

    internal static class UsersExtensions
    {

        /// <summary>
        /// Gets the student identifier asynchronous.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <param name="actionContext">The action context.</param>
        /// <returns></returns>
        /// <exception cref="Exception">The back-end header isn't added!</exception>
        /// <exception cref="NotSupportedException">The jwt doesn't have a sub</exception>
        /// <exception cref="System.Exception">The back-end header isn't added!</exception>
        public static string GetStudentId(this ClaimsPrincipal claimsPrincipal, HttpContext actionContext)
        {
            string studentId;

            if(claimsPrincipal.Identities.Any(i => !i.IsAuthenticated))
            {
                throw new Exception("User is not authenticated!");
            }

            if(claimsPrincipal.IsInRole(Defaults.Roles.BackendApplication))
            {
                string studentIdHeader = actionContext.Request.Headers.SingleOrDefault(h => h.Key == "StudentId")
                                                      .Value
                                                      .FirstOrDefault();

                if(string.IsNullOrWhiteSpace(studentIdHeader))
                {
                    throw new Exception("The back-end header isn't added!");
                }

                studentId = studentIdHeader;
            } else
            {
                string sub = claimsPrincipal.Claims.FirstOrDefault(c => c.Type.Equals("sub"))
                                            .Value;
                if(sub == null)
                {
                    throw new NotSupportedException("The jwt doesn't have a sub");
                }

                return sub;
            }

            return studentId;
        }

        /// <summary>
        /// Gets the context user.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <param name="userService">The user service.</param>
        /// <returns></returns>
        public static async Task<User> GetContextUser(this HttpContext actionContext, IUserService userService)
        {
            string identityProverId = actionContext.User.GetStudentId(actionContext);
            return await userService.GetUserByIdentityIdAsync(identityProverId);
        }


        /// <summary>
        /// Gets the user information asynchronous.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static User GetUserInformationAsync(this HttpContext actionContext, Config config)
        {
            string bearerToken = actionContext.Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
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
            if(jsonResponse == null ||
               !jsonResponse.ContainsKey("name") ||
               !jsonResponse.ContainsKey("email") ||
               !jsonResponse.ContainsKey("sub"))
            {
                return null;
            }
            User newUser = new User()
            {
                Name = (string) jsonResponse["name"],
                Email = (string) jsonResponse["email"],
                IdentityId = (string) jsonResponse["sub"]
            };
            return newUser ;
        }
    }

}
