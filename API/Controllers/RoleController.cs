using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Defaults;
using Services.Services;

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
        [Authorize(Policy = nameof(Defaults.Scopes.RoleRead))]
        public async Task<IActionResult> GetAllRoles()
        {
            List<Role> roles = await roleService.GetAllAsync();
            if(!roles.Any())
            {
                return NotFound();
            }

            return Ok(mapper.Map<IEnumerable<Role>, IEnumerable<RoleResourceResult>>(roles));
        }


        /// <summary>
        ///     Get all Roles.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = nameof(Defaults.Scopes.RoleRead))]
        public async Task<IActionResult> GetAllPossibleScopes()
        {

            //TODO Get from defaults

            return Ok(null);
        }




        /// <summary>
        ///     Get a Role.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.RoleRead))]
        public async Task<IActionResult> GetRole(int roleId)
        {
            if(roleId < 0)
            {
                return BadRequest("Invalid role Id");
            }

            Role role = await roleService.FindAsync(roleId);
            if(role == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Role, RoleResourceResult>(role));
        }

        /// <summary>
        ///     Create a Role.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync([FromBody] RoleResource roleResource)
        {
            if(roleResource == null)
            {
                return BadRequest("Role is null");
            }
            Role role = mapper.Map<RoleResource, Role>(roleResource);
            // TODO Validate existence of scopes.
            try
            {
                roleService.Add(role);
                roleService.Save();
                return Created(nameof(CreateRoleAsync), mapper.Map<Role, RoleResourceResult>(role));
            } catch
            {
                return BadRequest("Could not Create the Role.");
            }
        }

        /// <summary>
        ///     Update the Role
        /// </summary>

        /// <returns></returns>
        [HttpPut("{roleId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.RoleWrite))]
        public async Task<IActionResult> UpdateRole(int roleId, [FromBody] RoleResource roleResource)
        {
            Role role = await roleService.FindAsync(roleId);
            if(role == null)
            {
                return NotFound();
            }

            mapper.Map(roleResource, role);
            // TODO Validate existence of scopes.

            roleService.Update(role);
            roleService.Save();

            return Ok(mapper.Map<Role, RoleResourceResult>(role));
        }

        /// <summary>
        ///     deletes a Role.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{roleId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.RoleWrite))]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            if(await roleService.FindAsync(roleId) == null)
            {
                return NotFound();
            }
            await roleService.RemoveAsync(roleId);
            roleService.Save();
            return Ok();
        }




    }
}
