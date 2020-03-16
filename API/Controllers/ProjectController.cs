using System;
using System.Threading.Tasks;
using API.Resources;
using AutoMapper;
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
    public class ProjectController : ControllerBase
    {
		private readonly IProjectService _projectService;
		private readonly IMapper _mapper;

		/// <summary>
		/// Initialize a new instance of ProjectController
		/// </summary>
		/// <param name="projectService"></param>
		/// <param name="mapper"></param>
		public ProjectController(IProjectService projectService, IMapper mapper)
		{
			_projectService = projectService;
			_mapper = mapper;
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
				return NoContent();
			}

			return Ok(project);
		}


        /// <summary>
        /// Create a Project.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
		public IActionResult CreateProject([FromBody]ProjectResource projectResource)
		{
            if (projectResource == null) throw new ArgumentNullException(nameof(projectResource));
            if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			Project project = _mapper.Map<ProjectResource, Project>(projectResource);
            User u = new User();
            u.Name = "bob";
            project.User = u;
			try
			{

				_projectService.Add(project);
				_projectService.Save();
				return Created(nameof(CreateProject), _mapper.Map<Project, ProjectResourceResult>(project));
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
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			Project project = await _projectService.FindAsync(projectId);
			if (project == null)
			{
				return NoContent();
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
				return NoContent();
			}

			await _projectService.RemoveAsync(projectId);
			_projectService.Save();
			return Ok();
		}
	}
}