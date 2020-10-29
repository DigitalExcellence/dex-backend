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

using API.Common;
using API.Extensions;
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
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// This class is responsible for handling HTTP requests that are related
    /// to the users, for example creating, retrieving, updating or deleting.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IAuthorizationHelper authorizationHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class
        /// </summary>
        /// <param name="userService">The user service which is used to communicate with the logic layer.</param>
        /// <param name="mapper">The mapper which is used to convert the resources to the models to the resource results.</param>
        /// <param name="roleService">The role service which is used to communicate with the logic layer.</param>
        public UserController(IUserService userService, IMapper mapper, IRoleService roleService, IAuthorizationHelper authorizationHelper)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.roleService = roleService;
            this.authorizationHelper = authorizationHelper;
        }

        /// <summary>
        /// The method is responsible for retrieving the current user.
        /// </summary>
        /// <returns>The current user as user resource result.</returns>
        /// <response code="200">This endpoint returns the current user.</response>
        /// <response code="404">The 404 Not found status code is returned when the user could not be found.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(UserResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCurrentUser()
        {
            string identityId = HttpContext.User.GetIdentityId(HttpContext);
            User user = await userService.GetUserByIdentityIdAsync(identityId);
            if(user == null)
            {
                ProblemDetails problem = new ProblemDetails
                 {
                     Title = "Failed getting the user account.",
                     Detail = "The user could not be found in the database.",
                     Instance = "A4C4EEFA-1D3E-4E64-AF00-76C44D805D98"
                };
                return NotFound(problem);
            }
            return Ok(mapper.Map<User, UserResourceResult>(user));
        }

        /// <summary>
        /// This method is responsible for retrieving a user account.
        /// </summary>
        /// <param name="userId">the user identifier which is used for searching a user.</param>
        /// <returns>This method returns the user resource result.</returns>
        /// <response code="200">This endpoint returns the user with the specified user id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the user id is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when the user with the specified id could not be found.</response>
        [HttpGet("{userId}")]
        [Authorize]
        [ProducesResponseType(typeof(UserResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUser(int userId)
        {
            User currentUser = await HttpContext.GetContextUser(userService).ConfigureAwait(false);
            bool isAllowed = await authorizationHelper.UserIsAllowed(currentUser,
                                                               nameof(Defaults.Scopes.UserRead),
                                                               nameof(Defaults.Scopes.RequestUserRead),
                                                               userId);

            if(!isAllowed)
                return Forbid();

            if(userId < 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the user account.",
                    Detail = "The user id is less then zero and therefore cannot exist in the database.",
                    Instance = "EAF7FEA1-47E9-4CF8-8415-4D3BC843FB71",
                };
                return BadRequest(problem);
            }

            User user = await userService.FindAsync(userId);
            if(user == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the user account.",
                    Detail = "The user could not be found in the database.",
                    Instance = "140B718F-9ECD-4F68-B441-F85C1DC7DC32"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<User, UserResourceResult>(user));
        }

        /// <summary>
        /// This method is responsible for creating the account.
        /// </summary>
        /// <param name="accountResource">The account resource which is used for creating the account.</param>
        /// <returns>This method returns the created user as user resource result.</returns>
        /// <response code="200">This endpoint returns the created user.</response>
        /// <response code="400">The 400 Bad Request status code is return when saving the user to the database failed.</response>
        [HttpPost]
        [Authorize(Policy = nameof(Defaults.Scopes.UserWrite))]
        [ProducesResponseType(typeof(UserResourceResult), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAccountAsync([FromBody] UserResource accountResource)
        {
            User user = mapper.Map<UserResource, User>(accountResource);
            Role registeredUserRole = (await roleService.GetAll()).FirstOrDefault(i => i.Name == nameof(Defaults.Roles.RegisteredUser));
            user.Role = registeredUserRole;

            try
            {
                userService.Add(user);
                userService.Save();
                return Created(nameof(CreateAccountAsync), mapper.Map<User, UserResourceResult>(user));
            } catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception");

                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to create user account.",
                    Detail = "Failed saving the user account to the database.",
                    Instance = "D8C786C1-9E6D-4D36-83F4-A55D394B5017"
                };
                return BadRequest(problem);
            }
        }

        /// <summary>
        /// This method is responsible for updating the account.
        /// </summary>
        /// <param name="userId">The user identifier which is used for searching the user.</param>
        /// <param name="userResource">The user resource which is used for updating the user.</param>
        /// <returns>This method returns the updated user as user resource result.</returns>
        /// <response code="200">This endpoint returns the updated user.</response>
        /// <response code="401">The 401 Unauthorized status code is returned when the user is not allowed to update the account.</response>
        /// <response code="404">The 404 Not Found status code is returned when the user with the specified id could not be found.</response>
        [HttpPut("{userId}")]
        [Authorize]
        [ProducesResponseType(typeof(UserResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateAccount(int userId, [FromBody] UserResource userResource)
        {
            User currentUser = await HttpContext.GetContextUser(userService).ConfigureAwait(false);
            bool isAllowed = userService.UserHasScope(currentUser.IdentityId, nameof(Defaults.Scopes.UserWrite));

            if(currentUser.Id != userId && !isAllowed)
            {
                ProblemDetails problem = new ProblemDetails
                 {
                     Title = "Failed to edit the user.",
                     Detail = "The user is not allowed to edit this user.",
                     Instance = "E28BEBC0-AE7C-49F5-BDDC-3C13972B75D0"
                 };
                return Unauthorized(problem);
            }

            User user = await userService.FindAsync(userId);
            if(user == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the user account.",
                    Detail = "The database does not contain a user with that user id.",
                    Instance = "EF4DA55A-C31A-4BC4-AE30-098DEB0D3457"
                };
                return NotFound(problem);
            }

            mapper.Map(userResource, user);

            userService.Update(user);
            userService.Save();

            return Ok(mapper.Map<User, UserResourceResult>(user));
        }

        /// <summary>
        /// This method is responsible for deleting the current account.
        /// </summary>
        /// <returns>This method returns status code 200.</returns>
        /// <response code="200">This endpoint returns status code 200. The current account is deleted.</response>
        /// <response code="404">The 404 Not Found status code is returned when the current account could not be found.</response>
        [HttpDelete]
        [Authorize]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteAccount()
        {
            User user = await HttpContext.GetContextUser(userService).ConfigureAwait(false);

            if(await userService.FindAsync(user.Id) == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the user account.",
                    Detail = "The database does not contain a user with this user id.",
                    Instance = "C4C62149-FF9A-4E4C-8C9F-6BBF518BA085"
                };
                return NotFound(problem);
            }

            await userService.RemoveAsync(user.Id);
            userService.Save();
            return Ok();
        }

        /// <summary>
        /// This method is responsible for deleting a user account.
        /// </summary>
        /// <returns>This method returns status code 200.</returns>
        /// <response code="200">This endpoint returns status code 200. The account with the specified id is deleted.</response>
        /// <response code="401">The 401 Unauthorized status code is returned when the user is not allowed to delete the account.</response>
        /// <response code="404">The 404 Not Found status code is returned when the user with the specified id could not be found.</response>
        [HttpDelete("{userId}")]
        [Authorize]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteAccount(int userId)
        {
            User user = await HttpContext.GetContextUser(userService).ConfigureAwait(false);
            bool isAllowed = await authorizationHelper.UserIsAllowed(user,
                                                                     nameof(Defaults.Scopes.UserWrite),
                                                                     nameof(Defaults.Scopes.RequestUserWrite),
                                                                     userId);

            if(user.Id != userId && !isAllowed)
            {
                ProblemDetails problem = new ProblemDetails
                 {
                     Title = "Failed to delete the user.",
                     Detail = "The user is not allowed to delete this user.",
                     Instance = "26DA6D58-DB7B-467D-90AA-69EFBF55A83C"
                 };
                return Unauthorized(problem);
            }

            if(await userService.FindAsync(userId) == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the user account.",
                    Detail = "The database does not contain a user with this user id.",
                    Instance = "C4C62149-FF9A-4E4C-8C9F-6BBF518BA085"
                };
                return NotFound(problem);
            }

            await userService.RemoveAsync(userId);
            userService.Save();
            return Ok();
        }
    }
}
