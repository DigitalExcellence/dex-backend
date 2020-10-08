using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Defaults;
using Serilog;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static Models.Defaults.Defaults;

namespace API.Controllers
{
    /// <summary>
    /// This class is responsible for handling HTTP requests that are related
    /// to the roles, for example creating, retrieving, updating or deleting.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IRoleService roleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class
        /// </summary>
        /// <param name="roleService">The role service which is used to communicate with the logic layer.</param>
        /// <param name="userService">The user service which is used to communicate with the logic layer.</param>
        /// <param name="mapper">The mapper which is used to convert the resources to the model to the resource result.</param>
        public RoleController(IRoleService roleService, IUserService userService, IMapper mapper)
        {
            this.roleService = roleService;
            this.userService = userService;
            this.mapper = mapper;
        }

        /// <summary>
        /// This method is responsible for retrieving all roles.
        /// </summary>
        /// <returns>This method returns a list of role resource results.</returns>
        /// <response code="200">This endpoint returns a list of roles.</response>
        [HttpGet]
        [Authorize(Policy = nameof(Scopes.RoleRead))]
        [ProducesResponseType(typeof(IEnumerable<RoleResourceResult>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllRoles()
        {
            List<Role> roles = roles = await roleService.GetAllAsync()
                                                        .ConfigureAwait(false);

            return Ok(mapper.Map<IEnumerable<Role>, IEnumerable<RoleResourceResult>>(roles));
        }

        /// <summary>
        /// This method is responsible for retrieving all scopes.
        /// </summary>
        /// <returns>This method returns a list of valid scopes.</returns>
        /// <response code="200">This endpoint returns a list of scopes.</response>
        /// <response code="404">The 404 Not Found status code is returned when no scopes are found.</response>
        [HttpGet("Scopes")]
        [Authorize(Policy = nameof(Scopes.RoleRead))]
        [ProducesResponseType(typeof(List<string>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public IActionResult GetAllPossibleScopes()
        {
            List<string> scopeList;

            try
            {
                scopeList = roleService.GetValidScopes();
            } catch(Exception)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "No valid Scopes found",
                                             Detail = "Unexpected error occurred.",
                                             Instance = "C796E78E-75C1-4CE3-8E43-4A32997B4852"
                };
                return BadRequest(problem);
            }
            
            return Ok(scopeList);
        }

        /// <summary>
        /// This method is responsible for retrieving a single role.
        /// </summary>
        /// <returns>This method return the role resource result.</returns>
        /// <response code="200">This endpoint returns the role with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified role id is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when no role is found with the specified role id.</response>
        [HttpGet("{roleId}")]
        [Authorize(Policy = nameof(Scopes.RoleRead))]
        [ProducesResponseType(typeof(RoleResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
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
        /// This method is responsible for creating the role.
        /// </summary>
        /// <param name="roleResource">The role resource which is used to create a role.</param>
        /// <returns>This method returns the created role resource result.</returns>
        /// <response code="201">This endpoint returns the created role.</response>
        /// <response code="400">The 400 Bad Request status code is returned when unable to create role.</response>
        [HttpPost]
        [Authorize(Policy = nameof(Scopes.RoleWrite))]
        [ProducesResponseType(typeof(RoleResourceResult), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
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
        /// This method is responsible for updating the role.
        /// </summary>
        /// <param name="roleId">The role identifier which is used for searching the role.</param>
        /// <param name="roleResource">The role resource which is used to update the role.</param>
        /// <returns>This method returns the updated role resource result.</returns>
        /// <response code="200">This endpoint returns the updated role.</response>
        /// <response code="400">The 400 Bad Request status code is returned when a role scope is not valid.</response>
        /// <response code="404">The 404 Not Found status code is returned when the role with the specified id could not be found.</response>
        [HttpPut("{roleId}")]
        [Authorize(Policy = nameof(Scopes.RoleWrite))]
        [ProducesResponseType(typeof(RoleResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
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
        /// This method is responsible for deleting the role.
        /// </summary>
        /// <param name="roleId">The role identifier which is used for searching the role.</param>
        /// <returns>This method returns status code 200.</returns>
        /// <response code="200">This endpoint returns status code 200. Role is deleted.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the role is assigned to a user.</response>
        /// <response code="401">The 401 Unauthorized status code is returned when the user is not allowed to delete roles.</response>
        /// <response code="404">The 404 Not Found status code is returned when the role with the specified id could not be found.</response>
        [HttpDelete("{roleId}")]
        [Authorize(Policy = nameof(Scopes.RoleWrite))]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
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
        /// This method is responsible for setting the role.
        /// </summary>
        /// <param name="userId">The user identifier which is used for searching the user.</param>
        /// <param name="roleId">The role identifier which is used for searching the role.</param>
        /// <returns>This method returns the user resource result.</returns>
        /// <response code="200">This endpoint returns the updated user.</response>
        /// <response code="404">The 404 Not Found status code is returned when the specified role or user could not be found.</response>
        [HttpPut("setRole")]
        [Authorize(Policy = nameof(Scopes.RoleWrite))]
        [ProducesResponseType(typeof(UserResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
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
