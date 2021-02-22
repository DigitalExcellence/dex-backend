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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Defaults;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace API.Extensions
{

    /// <summary>
    ///     Easily grab scopes
    /// </summary>
    public class AuthorizeScopeAttribute : TypeFilterAttribute
    {

        /// <summary>
        ///     Currently deprecated
        ///     Initializes a new instance of the <see cref="AuthorizeScopeAttribute" /> class.
        /// </summary>
        /// <param name="claimValue">The claim value.</param>
        public AuthorizeScopeAttribute(string claimValue) : base(typeof(AuthorizeScopeFilter))
        {
            Arguments = new object[] {new Claim("scope", claimValue)};
        }

    }

    /// <summary>
    ///     Deprecated attribute filter.
    /// </summary>
    public class AuthorizeScopeFilter : IAuthorizationFilter
    {

        private readonly Claim claim;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthorizeScopeFilter" /> class.
        /// </summary>
        /// <param name="claim">The claim.</param>
        public AuthorizeScopeFilter(Claim claim)
        {
            this.claim = claim;
        }

        /// <summary>
        ///     Called early in the filter pipeline to confirm request is authorized.
        /// </summary>
        /// <param name="context">
        ///     The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext" />The current
        ///     context.
        /// </param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            UserService userService =
                context.HttpContext.RequestServices.GetService(typeof(UserService)) as UserService;
            bool hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value);

            //Get all scopes from the user
            IEnumerable<Claim> scopes =
                context.HttpContext.User.FindAll("scope");
            bool hasIdentityClaim = false;

            foreach(Claim scope in scopes)
            {
                // Check if the selected scope is a category
                FieldInfo scopeCategory = typeof(Defaults.ScopeCategories).GetField(scope.Value);
                if(scopeCategory != null)
                {
                    // get every scope in the category
                    string[] scopesInCategory = scopeCategory.GetValue(scopeCategory)
                                                             .ToString()
                                                             .Replace(":", "")
                                                             .Split(" ");
                    foreach(string defaultScope in scopesInCategory)
                    {
                        // check if the scope in the category is equal to the requested scope
                        if(string.Equals(defaultScope,
                                         claim.Value,
                                         StringComparison.OrdinalIgnoreCase))
                        {
                            hasIdentityClaim = true;
                        }
                    }
                } else
                {
                    if(scope.Value == claim.Value)
                    {
                        hasIdentityClaim = true;
                    }
                }
            }

            if(!hasClaim &&
               !hasIdentityClaim)
            {
                context.Result = new ForbidResult();
            }
        }

    }

}
