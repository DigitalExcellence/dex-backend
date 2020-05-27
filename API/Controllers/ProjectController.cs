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

using Models;
using Models.Defaults;
using Services.Services;
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
        private readonly SourceManagerService sourceManagerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectController"/> class.
        /// </summary>
        /// <param name="projectService">The project service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="sourceManagerService">The source manager service.</param>
        public ProjectController(IProjectService projectService, IUserService userService, IMapper mapper, SourceManagerService sourceManagerService)
        {
            this.projectService = projectService;
            this.userService = userService;
            this.mapper = mapper;
            this.sourceManagerService = sourceManagerService;
        }

        /// <summary>
        ///     Get all projects.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            List<Project> projects = await projectService.GetAllWithUsersAsync().ConfigureAwait(false);
            if(projects.Count == 0)
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

            Project project = await projectService.FindWithUserAndCollaboratorsAsync(projectId).ConfigureAwait(false);
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
        [Authorize]
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
            project.User = await HttpContext.GetContextUser(userService).ConfigureAwait(false);
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
        [Authorize]
        public async Task<IActionResult> UpdateProject(int projectId, [FromBody] ProjectResource projectResource)
        {
            Project project = await projectService.FindAsync(projectId).ConfigureAwait(false);
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

            User user = await HttpContext.GetContextUser(userService).ConfigureAwait(false);
            bool isAllowed = userService.UserHasScope(user.IdentityId, nameof(Defaults.Scopes.ProjectWrite));

            if(!(project.UserId == user.Id || isAllowed))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to edit the project.",
                    Detail = "The user is not allowed to edit the project.",
                    Instance = "2E765D18-8EBC-4117-8F9E-B800E8967038"
                };
                return Unauthorized(problem);
            }

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
            Project project = await projectService.FindAsync(projectId).ConfigureAwait(false);
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
            
            User user = await HttpContext.GetContextUser(userService).ConfigureAwait(false);
            bool isAllowed = userService.UserHasScope(user.IdentityId, nameof(Defaults.Scopes.ProjectWrite));

            if(!(project.UserId == user.Id || isAllowed))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to delete the project.",
                    Detail = "The user is not allowed to delete the project.",
                    Instance = "D0363680-5B4F-40A1-B381-0A7544C70164"
                };
                return Unauthorized(problem);
            }

            await projectService.RemoveAsync(projectId).ConfigureAwait(false);
            projectService.Save();
            return Ok();
        }


        /// <summary>
        /// Gets the wizard information.
        /// </summary>
        /// <param name="sourceUrl">The source URL.</param>
        /// <returns></returns>
        [HttpGet("wizard")]
        [Authorize]
        public async Task<IActionResult> GetWizardInformation(string sourceUrl)
        {
            if(string.IsNullOrEmpty(sourceUrl))
            {
                ProblemDetails problem = new ProblemDetails
                 {
                     Title = "Source url is null or empty.",
                     Detail = "the incoming source url is not valid.",
                     Instance = "6D63D9FA-91D6-42D5-9ACB-461FBEB0D2ED"
                };
                return BadRequest(problem);
            }
            Project project = sourceManagerService.FetchProject(sourceUrl);
            if(project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
    }
}
