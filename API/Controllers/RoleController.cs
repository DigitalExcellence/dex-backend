using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Defaults;
using Serilog;
using Services.Services;
using static Models.Defaults.Defaults;

namespace API.Controllers
{
    /// <summary>
    ///     This controller handles the CRUD roles
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IRoleService roleService;

        /// <summary>
        /// Initialize a new instance of RoleController
        /// </summary>
        /// <param name="roleService"></param>
        /// <param name="userService"></param>
        /// <param name="mapper"></param>
        public RoleController(IRoleService roleService, IUserService userService, IMapper mapper)
        {
            this.roleService = roleService;
            this.userService = userService;
            this.mapper = mapper;
        }

        /// <summary>
        ///     Get all Roles.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = nameof(Scopes.RoleRead))]
        public async Task<IActionResult> GetAllRoles()
        {
            List<Role> roles = await roleService.GetAllAsync().ConfigureAwait(false);
            if(roles.Count == 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting all roles.",
                    Detail = "There where no roles found in the database.",
                    Instance = "3EB1E953-96D7-45FE-8C5C-05306AF8D060"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<IEnumerable<Role>, IEnumerable<RoleResourceResult>>(roles));
        }

        /// <summary>
        ///     Get all Scopes.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Scopes")]
        [Authorize(Policy = nameof(Scopes.RoleRead))]
        public IActionResult GetAllPossibleScopes()
        {
            List<string> scopeList = roleService.GetValidScopes();
            if(scopeList.Count == 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "No valid Scopes found",
                    Detail = "There where no valid scopes found.",
                    Instance = "DEB2161D-A8E7-4AAE-BB0F-CDB3CA5D5B9E"
                };
                return NotFound(problem);
            }
            return Ok(scopeList);
        }

        /// <summary>
        ///     Get a Role.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        [Authorize(Policy = nameof(Scopes.RoleRead))]
        public async Task<IActionResult> GetRole(int roleId)
        {
            if(roleId < 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting role.",
                    Detail = "The Id is smaller then 0 and therefore it could never be a valid role id.",
                    Instance = "5024ADDA-6DE2-4B49-896A-526E8EC4313D"
                };
                return BadRequest(problem);
            }

            Role role = await roleService.FindAsync(roleId).ConfigureAwait(false);
            if(role == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting role.",
                    Detail = "The role could not be found in the database.",
                    Instance = "1739EFA6-3F31-4C88-B596-74DA403AC51B"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<Role, RoleResourceResult>(role));
        }

        /// <summary>
        /// Creates the role asynchronous.
        /// </summary>
        /// <param name="roleResource">The role resource.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = nameof(Scopes.RoleWrite))]
        public async Task<IActionResult> CreateRoleAsync([FromBody]RoleResource roleResource)
        {
            if(roleResource == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to create a new role.",
                    Detail = "The specified role resource was null",
                    Instance = "ABA3B997-1B80-47FC-A72B-69BC0D8DFA93"
                };
                return BadRequest(problem);
            }
            Role role = mapper.Map<RoleResource, Role>(roleResource);

            foreach(RoleScope roleScope in role.Scopes)
            {
                if(!roleService.isValidScope(roleScope.Scope))
                {
                    ProblemDetails problem = new ProblemDetails
                    {
                        Title = "Failed to create a new role.",
                        Detail = $"The specified scope: {roleScope.Scope} is not valid.",
                        Instance = "1F40D851-8A4C-41F6-917C-D876970D825F"
                    };
                    return BadRequest(problem);
                }
            }

            try
            {
                await roleService.AddAsync(role).ConfigureAwait(false);
                roleService.Save();
                return Created(nameof(CreateRoleAsync), mapper.Map<Role, RoleResourceResult>(role));
            }
            catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception");

                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to save the new role.",
                    Detail = "There was a problem while saving the role to the database.",
                    Instance = "D56DBE55-57A1-4655-99C5-4F4ECEEE3BE4"
                };
                return BadRequest(problem);
            }
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="roleResource">The role resource.</param>
        /// <returns></returns>
        [HttpPut("{roleId}")]
        [Authorize(Policy = nameof(Scopes.RoleWrite))]
        public async Task<IActionResult> UpdateRole(int roleId, RoleResource roleResource)
        {
            Role currentRole = await roleService.FindAsync(roleId).ConfigureAwait(false);
            if(currentRole == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to update the role.",
                    Detail = "The specified role could not be found in the database",
                    Instance = "8F167FDF-3B2B-4E71-B3D0-AA2B1C1CE2C3"
                };
                return NotFound(problem);
            }
            mapper.Map<RoleResource, Role>(roleResource,currentRole);
            foreach(RoleScope roleScope in currentRole.Scopes)
            {
                if(!roleService.isValidScope(roleScope.Scope))
                {
                    ProblemDetails problem = new ProblemDetails
                    {
                        Title = "Failed to update the role",
                        Detail = $"The specified scope is not a valid scope: {roleScope.Scope}.",
                        Instance = "E0BB725C-4013-4B0E-AEBC-857F1F75B29C"
                    };
                    return BadRequest(problem);
                }
            }


            roleService.Update(currentRole);
            roleService.Save();

            return Ok(mapper.Map<Role, RoleResourceResult>(currentRole));
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [HttpDelete("{roleId}")]
        [Authorize(Policy = nameof(Scopes.RoleWrite))]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            Role role = await roleService.FindAsync(roleId).ConfigureAwait(false);
            if(role == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to delete the role.",
                    Detail = "The role could not be found in the database.",
                    Instance = "CBC4C09D-DFEA-44D8-A310-2CE149BAD498"
                };
                return NotFound(problem);
            }
            if(role.Name == nameof(Defaults.Roles.Administrator) || role.Name == nameof(Defaults.Roles.RegisteredUser))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Not allowed.",
                    Detail = "For the stability of the program we need at least the registered user role and the admin role so these are not deletable..",
                    Instance = "CBC4C09D-DFEA-44D8-A310-2CE14123D498"
                };
                return Unauthorized(problem);
            }

            if(userService.UserWithRoleExists(role))
            {
                ProblemDetails problem = new ProblemDetails
                 {
                     Title = "Role is still assigned.",
                     Detail = "The role is still assigned to a user.",
                     Instance = "46E4AD0A-5947-4F9B-8001-A4D77CBC1A92"
                };
                return BadRequest(problem);
            }

            await roleService.RemoveAsync(role.Id).ConfigureAwait(false);
            roleService.Save();
            return Ok();
        }

        /// <summary>
        /// Sets the role.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [HttpPut("setRole")]
        [Authorize(Policy = nameof(Scopes.RoleWrite))]
        public async Task<IActionResult> SetRole(int userId, int roleId)
        {
            Role role = await roleService.FindAsync(roleId).ConfigureAwait(false);
            if(role == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to set role.",
                    Detail = "The role could not be found in the database.",
                    Instance = "A4D7DA5F-F47B-4FF6-8241-93D6808EEEDB"
                };
                return NotFound(problem);
            }
            User user = await userService.FindAsync(userId).ConfigureAwait(false);
            if(user == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to set role.",
                    Detail = "The user could not be found in the database.",
                    Instance = "3C6F3A13-5045-41FF-8C77-FB3A7B49F597"
                };
                return NotFound(problem);
            }
            user.Role = role;
            userService.Update(user);
            userService.Save();

            return Ok(mapper.Map<User, UserResourceResult>(user));
        }
    }
}
