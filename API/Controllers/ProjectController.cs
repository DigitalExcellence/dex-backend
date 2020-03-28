using System;
using System.Threading.Tasks;
using System.Collections.Generic;
 using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Services;

namespace API.Controllers
{
	/// <summary>
	/// This controller handles the CRUD projects
	/// </summary>
	[Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
		private readonly IProjectService _projectService;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		/// <summary>
		/// Initialize a new instance of ProjectController
		/// </summary>
		/// <param name="projectService"></param>
		/// <param name="mapper"></param>
		public ProjectController(IProjectService projectService, IUserService userService, IMapper mapper)
		{
			_projectService = projectService;
            _userService = userService;
            _mapper = mapper;
		}

        /// <summary>
        /// Get all projects.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllProjects()
        {
            IEnumerable<Project> projects = await _projectService.GetAll();
            if (projects == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<Project>, IEnumerable<ProjectResourceResult>>(projects));
        }


        /// <summary>
        /// Get a project.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{projectId}")]
        //[Authorize(Roles = nameof(Defaults.Roles.Student), Policy = nameof(Defaults.Scopes.StudentRead))]
		public async Task<IActionResult> GetProject(int projectId)
		{
			if (projectId < 0)
			{
				return BadRequest("Invalid project Id");
			}
			Project project = await _projectService.FindAsync(projectId);
			if (project == null)
            {
                return NotFound();
            }

			return Ok(_mapper.Map<Project, ProjectResourceResult>(project));
		}

        /// <summary>
        /// Create a Project.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
		public async Task<IActionResult> CreateProjectAsync([FromBody]ProjectResource projectResource)
        {
            if (projectResource == null)
            {
                return BadRequest("Project is null");
            }
            Project project = _mapper.Map<ProjectResource, Project>(projectResource);
            //TODO: When login in frontend is functioning, get the user from JWT information
            User user = await _userService.GetUserAsync(1);
            project.User = user;
            project.UserId = user.Id;
			try
			{
                _projectService.Add(project);
				_projectService.Save();
				return Created(nameof(CreateProjectAsync), _mapper.Map<Project, ProjectResourceResult>(project));
			}
			catch
			{
				return BadRequest("Could not Create the Project");
			}
        }

        /// <summary>
        /// Update the Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="projectResource"></param>
        /// <returns></returns>
        [HttpPut("{projectId}")]
		public async Task<IActionResult> UpdateProject(int projectId, [FromBody]ProjectResource projectResource)
		{
            Project project = await _projectService.FindAsync(projectId);
			if (project == null)
            {
                return NotFound();
            }

			_mapper.Map<ProjectResource, Project>(projectResource, project);

			_projectService.Update(project);
			_projectService.Save();

			return Ok(_mapper.Map<Project, ProjectResourceResult>(project));
        }

        /// <summary>
        /// deletes a project.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{projectId}")]
		public async Task<IActionResult> DeleteProject(int projectId)
		{
			if (await _projectService.FindAsync(projectId) == null)
            {
                return NotFound();
            }
            await _projectService.RemoveAsync(projectId);
			_projectService.Save();
			return Ok();
		}
	}
}