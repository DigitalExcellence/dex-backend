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
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using RestSharp;
using RestSharp.Authenticators;
using Serilog;
using Services.Services;
using System.Linq;

namespace API.Controllers
{

    

    /// <summary>
    /// This class is responsible for handling HTTP requests that are related
    /// to the user tasks, for example creating, retrieving or deleting.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {

        private readonly IUserTaskService userTaskService;
        private readonly IUserService userService;
        private readonly IRoleService roleService;

        /// <summary>
        /// The constructor for user tasks
        /// </summary>
        /// <param name="userTaskService"> The user task service is responsible for getting and setting the tasks that users should follow up.</param>
        /// <param name="userService"> The user service is responsible for getting and setting users. </param>
        /// <param name="roleService"> The role service is responsible for getting and setting roles. </param>
        public UserTaskController(IUserTaskService userTaskService,
                                  IUserService userService,
                                  IRoleService roleService)
        {
            this.userTaskService = userTaskService;
            this.userService = userService;
            this.roleService = roleService;
        }

        /// <summary>
        /// Creates and returns all graduation user tasks for expecting graduation users.
        /// </summary>
        /// <returns> All user tasks which are created or open for graduation users. </returns>
        [HttpGet]
        public async Task<IActionResult> CreateUserTasksForGraduatingUsers()
        {
            List<UserTask> userTasks = await userTaskService.GetAllOpenGraduateUserTasks();

            return Ok(userTasks);
        }

        /// <summary>
        /// This endpoint is responsible for converting an account to Alumni. 
        /// </summary>
        /// <returns> The updated user. </returns>
        [HttpPut]
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

            UserTask userTask = await userTaskService.GetUserTasksForUser(user.Id);

            if(userTask == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "No graduation user task exists.",
                                             Detail = "The database does not contain a user task for graduating.",
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

            

            RestClient restClient = new RestClient("https://localhost:5005/");
            RestRequest restRequest = new RestRequest("Account/ChangeCredentials")
                                      {
                                          Method = Method.PUT
                                      };

            restRequest.AddHeader("password", Request.Headers.FirstOrDefault(h => h.Key == "password")
                                                     .Value.FirstOrDefault());
            restRequest.AddHeader("email",
                                  Request.Headers.FirstOrDefault(h => h.Key == "email")
                                         .Value.FirstOrDefault());
            restRequest.AddHeader("subjectId", user.IdentityId);
            IRestResponse response = restClient.Execute(restRequest);

            if(!response.IsSuccessful)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "IdentityServer had an error.",
                                             Detail = response.ErrorMessage,
                                             Instance = "94B362FE-F038-43AF-B2C3-462513D1C7F8"
                                         };
                return StatusCode(503, problem);
            }

            userTask.Status = UserTaskStatus.Completed;
            userTaskService.Update(userTask);

            user.Role = alumniRole;
            userService.Update(user);

            userTaskService.Save();
            userService.Save();
            return Ok(user);
        }




    }
}
