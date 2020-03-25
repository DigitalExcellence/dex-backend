using System.Threading.Tasks;
using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        /// <param name="userId"></param>
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
        /// Create a user account.
        /// </summary>
        /// <param name="accountResource"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateProject([FromBody] ProjectResource projectResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Project project = _mapper.Map<ProjectResource, Project>(projectResource);
            try
            {
                _projectService.Add(project);
                _projectService.Save();
                return Created(nameof(CreateProject), _mapper.Map<Project, ProjectResourceResult>(project));
            }
            catch
            {
                return BadRequest("Could not Create the User account");
            }
        }

        /// <summary>
        /// Update the User account.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userResource"></param>
        /// <returns></returns>
        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateProject(int projectId, [FromBody] ProjectResource projectResource)
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
        /// Gets the student information.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
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