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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Serilog;
using Services.Services;
using System;
using System.Threading.Tasks;

namespace API.Extensions
{

    /// <summary>
    ///     Initializes a new instance of the <see cref="ScopeRequirement" /> class.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Authorization.IAuthorizationRequirement" />
    public class ScopeRequirement : IAuthorizationRequirement
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScopeRequirement" /> class.
        /// </summary>
        /// <param name="scope">The scope.</param>
        public ScopeRequirement(string scope)
        {
            RequiredScope = scope;
        }

        /// <summary>
        ///     Gets or sets the required scope.
        /// </summary>
        /// <value>
        ///     The required scope.
        /// </value>
        public string RequiredScope { get; set; }

    }

    /// <summary>
    ///     This handler is called every time authorize is called with a policy created by the scope requirement class.
    /// </summary>
    public class ScopeRequirementHandler : AuthorizationHandler<ScopeRequirement>
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScopeRequirementHandler" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="userservice">The userservice.</param>
        public ScopeRequirementHandler(IHttpContextAccessor httpContextAccessor, IUserService userservice)
        {
            userService = userservice;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        ///     Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        /// <returns>CompletedTask.</returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       ScopeRequirement requirement)
        {
            string identityId;
            try
            {
                identityId = httpContextAccessor.HttpContext.User.GetIdentityId(httpContextAccessor.HttpContext);
            } catch(UnauthorizedAccessException e)
            {
                Log.Information(e, "User is not authenticated.");
                return Task.CompletedTask;
            }

            if(string.IsNullOrEmpty(identityId))
            {
                return Task.CompletedTask;
            }

            if(userService.UserHasScope(identityId, requirement.RequiredScope))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }

    }

}
