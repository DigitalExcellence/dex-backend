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

using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Defaults;
using Services.Services;
using System.Threading.Tasks;

namespace API.Controllers
{

    /// <summary>
    ///     This controller handles the user settings.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly IUserService userService;

        /// <summary>
        ///     Initialize a new instance of UserController
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="mapper"></param>
        public UserController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }


        /// <summary>
        ///     Get a user account.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.UserRead))]
        public async Task<IActionResult> GetUser(int userId)
        {
            if(userId < 0)
            {
                return BadRequest("Invalid user Id");
            }

            User user = await userService.FindAsync(userId);
            if(user == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<User, UserResourceResult>(user));
        }


        /// <summary>
        ///     Create a user account.
        /// </summary>
        /// <param name="accountResource"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = nameof(Defaults.Scopes.UserWrite))]
        public IActionResult CreateAccount([FromBody] UserResource accountResource)
        {
            User user = mapper.Map<UserResource, User>(accountResource);
            try
            {
                userService.Add(user);
                userService.Save();
                return Created(nameof(CreateAccount), mapper.Map<User, UserResourceResult>(user));
            } catch
            {
                return BadRequest("Could not Create the User account");
            }
        }

        /// <summary>
        ///     Update the User account.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userResource"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.UserWrite))]
        public async Task<IActionResult> UpdateAccount(int userId, [FromBody] UserResource userResource)
        {
            User user = await userService.FindAsync(userId);
            if(user == null)
            {
                return NotFound();
            }

            mapper.Map(userResource, user);

            userService.Update(user);
            userService.Save();

            return Ok(mapper.Map<User, UserResourceResult>(user));
        }

        /// <summary>
        ///     Gets the student information.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.UserWrite))]
        public async Task<IActionResult> DeleteAccount(int userId)
        {
            if(await userService.FindAsync(userId) == null)
            {
                return NotFound();
            }

            await userService.RemoveAsync(userId);
            userService.Save();
            return Ok();
        }

    }

}
