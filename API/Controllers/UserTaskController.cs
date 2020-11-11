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

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Services;

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

        /// <summary>
        /// The constructor for user tasks
        /// </summary>
        /// <param name="userTaskService"></param>
        public UserTaskController(IUserTaskService userTaskService)
        {
            this.userTaskService = userTaskService;
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




    }
}
