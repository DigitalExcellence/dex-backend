using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Defaults;
using Repositories;
using Services.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        [HttpPost("Activity")]
        [Authorize(Policy = nameof(Defaults.Roles.BackendApplication))]
        public async Task<IActionResult> ActivityAlgorithm(ActivityAlgorithmService activityAlgorithmService, ProjectRepository projectRepository)
        {
            IEnumerable<Project>  projects = activityAlgorithmService.CalculateAllProjects(await projectRepository.GetAll());
            return Ok(projects);
        }
    }
}
