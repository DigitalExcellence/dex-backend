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
    ///  Initializes a new instance of the <see cref="ScopeRequirement"/> class.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Authorization.IAuthorizationRequirement" />
    public class ScopeRequirement: IAuthorizationRequirement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeRequirement"/> class.
        /// </summary>
        /// <param name="scope">The scope.</param>
        public ScopeRequirement(string scope)
        {
            RequiredScope = scope;
        }
        /// <summary>
        /// Gets or sets the required scope.
        /// </summary>
        /// <value>
        /// The required scope.
        /// </value>
        public string RequiredScope { get; set; }
    }

    /// <summary>
    /// This handler is called every time authorize is called with a policy created by the scoperequirement class.
    /// </summary>
    public class ScopeRequirementHandler : AuthorizationHandler<ScopeRequirement>
    {
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;
        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeRequirementHandler"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="userservice">The userservice.</param>
        public ScopeRequirementHandler(IHttpContextAccessor httpContextAccessor, IUserService userservice)
        {
            userService = userservice;
            this.httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
        {
            string studentId = httpContextAccessor.HttpContext.User.GetStudentId(httpContextAccessor.HttpContext);
            if(string.IsNullOrEmpty(studentId))
            {
                return Task.CompletedTask;
            }

            if(userService.UserHasScope(studentId, requirement.RequiredScope))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
