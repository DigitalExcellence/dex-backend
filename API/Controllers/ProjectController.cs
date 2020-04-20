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
using Microsoft.EntityFrameworkCore.Internal;
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
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            List<Project> projects = await _projectService.GetAllWithUsersAsync();
            if(!projects.Any())
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<Project>, IEnumerable<ProjectResourceResult>>(projects));
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
                return BadRequest("Invalid project Id");
            }

            Project project = await _projectService.FindWithUserAndCollaboratorsAsync(projectId);
            if(project == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Project, ProjectResourceResult>(project));
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
                return BadRequest("Project is null");
            }
            Project project = _mapper.Map<ProjectResource, Project>(projectResource);
            User user = await _userService.GetUserAsync(projectResource.UserId);
            project.User = user;
            try
            {
                _projectService.Add(project);
                _projectService.Save();
                return Created(nameof(CreateProjectAsync), _mapper.Map<Project, ProjectResourceResult>(project));
            } catch
            {
                return BadRequest("Could not Create the Project");
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
            Project project = await _projectService.FindAsync(projectId);
            if(project == null)
            {
                return NotFound();
            }

            _mapper.Map(projectResource, project);

            _projectService.Update(project);
            _projectService.Save();

            return Ok(_mapper.Map<Project, ProjectResourceResult>(project));
        }

        /// <summary>
        ///     deletes a project.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{projectId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.ProjectWrite))]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            if(await _projectService.FindAsync(projectId) == null)
            {
                return NotFound();
            }
            await _projectService.RemoveAsync(projectId);
            _projectService.Save();
            return Ok();
        }

    }

}
