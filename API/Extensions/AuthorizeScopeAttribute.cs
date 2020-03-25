using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Defaults;

namespace API.Extensions
{
    public class AuthorizeScopeAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimValue"></param>
        public AuthorizeScopeAttribute(string claimValue) : base(typeof(AuthorizeScopeFilter))
        {
            Arguments = new object[] {new Claim("scope", claimValue)};
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AuthorizeScopeFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claim"></param>
        public AuthorizeScopeFilter(Claim claim)
        {
            _claim = claim;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);

            //Get all scopes from the user
            var scopes =
                context.HttpContext.User.FindAll("scope");
            bool hasIdentityClaim = false;

            foreach (Claim scope in scopes)
            {
                // Check if the selected scope is a category
                var scopeCategory = typeof(Defaults.ScopeCategories).GetField(scope.Value);
                if (scopeCategory != null)
                {
                    // get every scope in the category
                    var scopesInCategory = scopeCategory.GetValue(scopeCategory).ToString().Replace(":", "").Split(" ");
                    foreach (string defaultScope in scopesInCategory)
                    {
                        // check if the scope in the category is equal to the requested scope
                        if (string.Equals(defaultScope, _claim.Value,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            hasIdentityClaim = true;
                        }
                    }
                }
                else
                {
                    if (scope.Value == _claim.Value)
                    {
                        hasIdentityClaim = true;
                    }
                }
            }

            if (!hasClaim && !hasIdentityClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}