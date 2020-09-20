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
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Defaults;
using Serilog;
using Services.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// This controller handles the CRUD projects
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IProjectService projectService;
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectController"/> class.
        /// </summary>
        /// <param name="projectService">The project service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="mapper">The mapper.</param>
        public ProjectController(IProjectService projectService, IUserService userService, IMapper mapper)
        {
            this.projectService = projectService;
            this.userService = userService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get all projects.
        /// </summary>
        /// <param name="projectFilterParamsResource">The parameters to filter, sort and paginate the projects</param>
        /// <returns>The project result resource.</returns>
        /// <response code="200">Returns the Project Result Resource</response>
        /// <response code="400">If values inside project filter params resource are invalid</response>
        [HttpGet]
        [ProducesResponseType(typeof(ProjectResultsResource), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllProjects([FromQuery] ProjectFilterParamsResource projectFilterParamsResource)
        {
            ProblemDetails problem = new ProblemDetails
                                     {
                                         Title = "Invalid search request."
                                     };
            if(projectFilterParamsResource.Page != null &&
               projectFilterParamsResource.Page < 1)
            {
                problem.Detail = "The page number cannot be smaller then 1.";
                problem.Instance = "65EB6EF1-2CF4-4F7B-8A0A-C047C701337A";
                return BadRequest(problem);
            }
            if(projectFilterParamsResource.SortBy != null &&
               projectFilterParamsResource.SortBy != "name" &&
               projectFilterParamsResource.SortBy != "created" &&
               projectFilterParamsResource.SortBy != "updated")
            {
                problem.Detail = "Invalid sort value: Use \"name\", \"created\" or \"updated\".";
                problem.Instance = "5CE2F569-C0D5-4179-9299-62916270A058";
                return BadRequest(problem);
            }
            if(projectFilterParamsResource.SortDirection != null &&
               projectFilterParamsResource.SortDirection != "asc" &&
               projectFilterParamsResource.SortDirection != "desc")
            {
                problem.Detail = "Invalid sort direction: Use \"asc\" or \"desc\".";
                problem.Instance = "3EE043D5-070B-443A-A951-B252A1BB8EF9";
                return BadRequest(problem);
            }

            ProjectFilterParams projectFilterParams = mapper.Map<ProjectFilterParamsResource, ProjectFilterParams>(projectFilterParamsResource);
            IEnumerable<Project> projects = await projectService.GetAllWithUsersAsync(projectFilterParams);
            IEnumerable<ProjectResultResource> results =
                mapper.Map<IEnumerable<Project>, IEnumerable<ProjectResultResource>>(projects);

            ProjectResultsResource resultsResource = new ProjectResultsResource()
                                                           {
                                                               Results = results.ToArray(),
                                                               Count = results.Count(),
                                                               TotalCount = await projectService.ProjectsCount(projectFilterParams),
                                                               Page = projectFilterParams.Page,
                                                               TotalPages =
                                                                   await projectService.GetProjectsTotalPages(projectFilterParams)
                                                           };

            return Ok(resultsResource);
        }

        /// <summary>
        ///     Get a project.
        /// </summary>
        /// <returns>The project resource result.</returns>
        /// <response code="200">Returns the project resource result with the given Id</response>
        /// <response code="400">If the specified id is invalid</response>
        /// <response code="404">If the project could not be found with the specified id</response>
        [HttpGet("{projectId}")]
        [ProducesResponseType(typeof(ProjectResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
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
        /// <returns>The project resource result.</returns>
        /// <response code="200">Returns the created project resource result</response>
        /// <response code="400">If project resource is not specified or failed to save project to the database</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ProjectResourceResult), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
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
            } catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception");


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
        /// Updates the project with the specified identifier.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="projectResource">The project resource.</param>
        /// <returns>The project resource result.</returns>
        /// <response code="200">Returns the updated project resource result</response>
        /// <response code="401">If project is not owned by the user or the user has not the correct permission to update</response>
        /// <response code="404">If project to update is not found</response>
        [HttpPut("{projectId}")]
        [Authorize]
        [ProducesResponseType(typeof(ProjectResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
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
        /// deletes a project.
        /// </summary>
        /// <returns>Status code 200.</returns>
        /// <response code="200">Returns 200, project is deleted</response>
        /// <response code="401">If project is not owned by the user or the user has not the correct permission to delete</response>
        /// <response code="404">If project to delete was not found</response>
        [HttpDelete("{projectId}")]
        [Authorize]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
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
    }
}
