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
        /// </summary>
        /// <param name="claimValue"></param>
        public AuthorizeScopeAttribute(string claimValue) : base(typeof(AuthorizeScopeFilter))
        {
            Arguments = new object[] {new Claim("scope", claimValue)};
        }

    }

    /// <summary>
    /// </summary>
    public class AuthorizeScopeFilter : IAuthorizationFilter
    {

        private readonly Claim claim;

        /// <summary>
        /// </summary>
        /// <param name="claim"></param>
        public AuthorizeScopeFilter(Claim claim)
        {
            this.claim = claim;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
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
