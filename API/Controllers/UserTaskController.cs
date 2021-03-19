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

using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Defaults;
using Services.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Controllers
{

    /// <summary>
    ///     This class is responsible for handling HTTP requests that are related
    ///     to the user tasks, for example creating, retrieving or deleting.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {

        private readonly HttpClient identityHttpClient;
        private readonly IRoleService roleService;
        private readonly IUserService userService;

        private readonly IUserTaskService userTaskService;

        /// <summary>
        ///     The constructor for user tasks
        /// </summary>
        /// <param name="userTaskService">
        ///     The user task service is responsible for getting and setting the tasks that users should
        ///     follow up.
        /// </param>
        /// <param name="userService"> The user service is responsible for getting and setting users. </param>
        /// <param name="roleService"> The role service is responsible for getting and setting roles. </param>
        /// <param name="clientFactory"> The client factory is a HttpClient which is used to communicate with the identity server. </param>
        public UserTaskController(IUserTaskService userTaskService,
                                  IUserService userService,
                                  IRoleService roleService,
                                  IHttpClientFactory clientFactory)
        {
            this.userTaskService = userTaskService;
            this.userService = userService;
            this.roleService = roleService;
            identityHttpClient = clientFactory.CreateClient("identityclient");
        }

        /// <summary>
        ///     Creates and returns all graduation user tasks for expecting graduation users.
        /// </summary>
        /// <param name="withinAmountOfMonths">
        ///     This value determines the range of graduating users. The range is between now and
        ///     entered number of months.
        /// </param>
        /// <returns> All user tasks which are created or open for graduation users. </returns>
        /// <response code="200">This endpoint returns a list of user tasks.</response>
        [HttpGet("CreateUserTasks/{withinAmountOfMonths}")]
        [ProducesResponseType(typeof(List<UserTask>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> CreateUserTasksForGraduatingUsers(int withinAmountOfMonths)
        {
            // First check if worker service is trying to execute method to prevent internal server error.
            bool isAllowed = HttpContext.User.HasClaim("client_role", Defaults.Roles.BackendApplication);

            if(isAllowed == false)
            {
                User currentUser = await HttpContext.GetContextUser(userService)
                                                    .ConfigureAwait(false);
                if(currentUser.Role.Name == Defaults.Roles.Administrator)
                {
                    isAllowed = true;
                }
            }

            if(isAllowed)
            {
                List<UserTask> userTasks = await userTaskService.GetAllOpenGraduateUserTasks(withinAmountOfMonths);

                return Ok(userTasks);
            }

            return Forbid();
        }

        /// <summary>
        ///     Creates and returns all graduation user tasks for expecting graduation users.
        /// </summary>
        /// <returns> All user tasks which are created or open for graduation users. </returns>
        /// <response code="200">This status code is returned when the user tasks were found successfully.</response>
        /// <response code="404">This status code is returned when no user was found.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserTask>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserTasksForCurrentUser()
        {
            User currentUser = await HttpContext.GetContextUser(userService)
                                                .ConfigureAwait(false);
            if(currentUser == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting the user account.",
                                             Detail = "The user could not be found in the database.",
                                             Instance = "548DA96F-0183-483F-8CE9-2848A868DC57"
                                         };
                return NotFound(problem);
            }

            List<UserTask> userTasks = await userTaskService.GetUserTasksForUser(currentUser.Id);

            return Ok(userTasks);
        }

        /// <summary>
        ///     This endpoint is responsible for converting an account to Alumni.
        ///     New credentials in the headers due to security.
        /// </summary>
        /// <returns> The updated user. </returns>
        /// <response code="200">This endpoint returns the converted user.</response>
        /// <response code="404">The 404 Not found status code is returned when the user could not be found.</response>
        /// <response code="503">
        ///     The 503 Service unavailable status code is returned if the identity server cannot execute the
        ///     request.
        /// </response>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(typeof(User), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.ServiceUnavailable)]
        public async Task<IActionResult> ConvertToAlumni()
        {
            User user = await HttpContext.GetContextUser(userService)
                                         .ConfigureAwait(false);

            if(await userService.FindAsync(user.Id) == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting the user account.",
                                             Detail = "The database does not contain a user with this user id.",
                                             Instance = "598E61EC-1C0F-4ED2-AC42-F5B5503D4A5E"
                                         };
                return NotFound(problem);
            }

            List<UserTask> userTasks = await userTaskService.GetUserTasksForUser(user.Id);
            UserTask userTask = userTasks.Find(u => u.Type == UserTaskType.GraduationReminder);

            if(userTask == null ||
               userTask.Status == UserTaskStatus.Completed)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "No graduation user task exists.",
                                             Detail =
                                                 "The database does not contain a (uncompleted) user task for graduating.",
                                             Instance = "A10E4AE8-633D-4334-BADA-99B7AD077B6D"
                                         };
                return NotFound(problem);
            }

            List<Role> roles = await roleService.GetAllAsync();
            Role alumniRole = roles.Find(r => r.Name == "Alumni");

            if(alumniRole == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Alumni role does not exist.",
                                             Detail = "The database does not contain a role for alumni.",
                                             Instance = "2DE31766-5D9C-4E41-908F-389A9A85F723"
                                         };
                return NotFound(problem);
            }


            // Rest call to Identity server to change credentials. Credentials are in the headers due to security issues.
            identityHttpClient.DefaultRequestHeaders.Add("password",
                                                         Request.Headers.FirstOrDefault(h => h.Key == "password")
                                                                .Value.FirstOrDefault());
            identityHttpClient.DefaultRequestHeaders.Add("email",
                                                         Request.Headers.FirstOrDefault(h => h.Key == "email")
                                                                .Value.FirstOrDefault());
            identityHttpClient.DefaultRequestHeaders.Add("subjectId", user.IdentityId);
            HttpResponseMessage resp = await identityHttpClient.PutAsync("ExternalAccount", new StringContent(""));

            if(!resp.IsSuccessStatusCode)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "IdentityServer had an error.",
                                             Detail = resp.ReasonPhrase,
                                             Instance = "94B362FE-F038-43AF-B2C3-462513D1C7F8"
                                         };
                return StatusCode(503, problem);
            }

            userTask.Status = UserTaskStatus.Completed;
            userTaskService.Update(userTask);


            user.Role = alumniRole;
            user.Email = Request.Headers.FirstOrDefault(h => h.Key == "email")
                                .Value.FirstOrDefault();
            userService.Update(user);

            userTaskService.Save();
            userService.Save();
            return Ok(user);
        }

        /// <summary>
        ///     Sets the user tasks to status mailed.
        /// </summary>
        /// <returns> All user tasks which are created or open for graduation users. </returns>
        /// <response code="200">This status code is returned when the user tasks were found successfully.</response>
        /// <response code="404">This status code is returned when no user was found.</response>
        [HttpPut("SetToMailed/{userTaskId}")]
        [Authorize(Policy = nameof(Defaults.Roles.BackendApplication))]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> SetUserTasksToStatusMailed(int userTaskId)
        {
            UserTask userTask = await userTaskService.FindAsync(userTaskId);

            if(userTask == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "User task was not found.",
                                             Detail = "User task with this ID does not exist.",
                                             Instance = "CA523709-D2CB-4EC5-BE31-32758D2587D0"
                                         };
                return NotFound(problem);
            }

            userTask.Status = UserTaskStatus.Mailed;
            userTaskService.Update(userTask);
            userTaskService.Save();

            return Ok();
        }

    }

}
