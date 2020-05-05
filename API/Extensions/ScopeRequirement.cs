using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Models;
using Services.Services;

namespace API.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Authorization.IAuthorizationRequirement" />
    public class ScopeRequirement: IAuthorizationRequirement
    {
        public ScopeRequirement(string scope)
        {
            RequiredScope = scope;
        }
        public string RequiredScope { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Authorization.AuthorizationHandler{Extensions.ScopeRequirement}" />
    public class ScopeRequirementHandler : AuthorizationHandler<ScopeRequirement>
    {
        IUserService userService;
        public ScopeRequirementHandler(IUserService userservice)
        {
            userService = userservice;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
        {
            string subjectId = context.User.FindFirst("sub").Value;

            var x = userService.UserHasScope(subjectId, requirement.RequiredScope);
            if(x)
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }

}
