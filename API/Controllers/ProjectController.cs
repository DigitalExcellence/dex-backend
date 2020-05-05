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
using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Models;
using Models.Defaults;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{

    /// <summary>
    ///     This controller handles the CRUD projects
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly IProjectService projectService;
        private readonly IUserService userService;

        /// <summary>
        /// Initialize a new instance of ProjectController
        /// </summary>
        /// <param name="projectService"></param>
        /// <param name="userService"></param>
        /// <param name="mapper"></param>
        public ProjectController(IProjectService projectService, IUserService userService, IMapper mapper)
        {
            this.projectService = projectService;
            this.userService = userService;
            this.mapper = mapper;
        }

        /// <summary>
        ///     Get all projects.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            List<Project> projects = await projectService.GetAllWithUsersAsync();
            if(!projects.Any())
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting all projects.",
                    Detail = "There where no projects found in the database.",
                    Instance = "1B9B7B05-1EBA-49B0-986F-B54EBA70EC0D"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<IEnumerable<Project>, IEnumerable<ProjectResourceResult>>(projects));
        }


        /// <summary>
        ///     Get a project.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProject(int projectId)
        {
            if(projectId < 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting project.",
                    Detail = "The Id is smaller then 0 and therefore it could never be a valid project id.",
                    Instance = "D590A4FE-FDBA-4AE5-B184-BC7395C45D4E"
                };
                return BadRequest(problem);
            }

            Project project = await projectService.FindWithUserAndCollaboratorsAsync(projectId);
            if(project == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting project.",
                    Detail = "The project could not be found in the database.",
                    Instance = "38516C41-4BFB-47BE-A759-1206BE6D2D13"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<Project, ProjectResourceResult>(project));
        }

        /// <summary>
        ///     Create a Project.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = nameof(Defaults.Scopes.ProjectWrite))]
        public async Task<IActionResult> CreateProjectAsync([FromBody] ProjectResource projectResource)
        {
            if(projectResource == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to create a new project.",
                    Detail = "The specified project resource was null.",
                    Instance = "8D3D9119-0D12-4631-B2DC-56494639A849"
                };
                return BadRequest(problem);
            }
            Project project = mapper.Map<ProjectResource, Project>(projectResource);
            User user = await userService.GetUserAsync(projectResource.UserId);
            project.User = user;
            try
            {
                projectService.Add(project);
                projectService.Save();
                return Created(nameof(CreateProjectAsync), mapper.Map<Project, ProjectResourceResult>(project));
            } catch
            {
                ProblemDetails problem = new ProblemDetails()
                {
                    Title = "Failed to save new project.",
                    Detail = "There was a problem while saving the project to the database.",
                    Instance = "9FEEF001-F91F-44E9-8090-6106703AB033"
                };
                return BadRequest(problem);
            }
        }

        /// <summary>
        ///     Update the Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="projectResource"></param>
        /// <returns></returns>
        [HttpPut("{projectId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.ProjectWrite))]
        public async Task<IActionResult> UpdateProject(int projectId, [FromBody] ProjectResource projectResource)
        {
            var x = HttpContext.User;
            Project project = await projectService.FindAsync(projectId);
            if(project == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to update project.",
                    Detail = "The specified project could not be found in the database.",
                    Instance = "6A123609-19A1-47F0-B789-3D8F2A52C0C6"
                };
                return NotFound(problem);
            }

            mapper.Map(projectResource, project);

            projectService.Update(project);
            projectService.Save();

            return Ok(mapper.Map<Project, ProjectResourceResult>(project));
        }

        /// <summary>
        ///     deletes a project.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{projectId}")]
        [Authorize]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            Project project = await projectService.FindAsync(projectId);
            if(project == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to delete the project.",
                    Detail = "The project could not be found in the database.",
                    Instance = "AF63CF48-ECAA-4996-BAA0-BF52926D12AC"
                };
                return NotFound(problem);
            }
            string identity = HttpContext.User.GetStudentId(HttpContext);
            bool isAllowed = userService.UserHasScope(identity, nameof(Defaults.Scopes.ProjectWrite));
            User user = await userService.GetUserByIdentityIdAsync(identity);

            if(!(project.UserId == user.Id || isAllowed))
            {
                return BadRequest("The user is not allowed to remove the project.");
            }

            await projectService.RemoveAsync(projectId);
            projectService.Save();
            return Ok();
        }

    }

}
