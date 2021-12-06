using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Defaults;
using Repositories;
using Services.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    ///     This class is responsible for handling HTTP requests that are related
    ///     to DeX algorithms, for example activating the project ActivityAlgorythm
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {

        private readonly IActivityAlgorithmService activityAlgorithmService;
        private readonly IProjectRepository projectRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlgorithmController" /> class
        /// </summary>
        /// <param name="activityAlgorithmService">The category service which is used to communicate with the logic layer.</param>
        /// <param name="projectRepository">The project category service which is used to communicate with the logic layer.</param>
        public AlgorithmController(IActivityAlgorithmService activityAlgorithmService, IProjectRepository projectRepository)
        {
            this.activityAlgorithmService = activityAlgorithmService;
            this.projectRepository = projectRepository;
        }

        /// <summary>
        /// This endpoint initiates the Activity Algorithm.
        /// </summary>
        /// <returns>HttpStatusCode</returns>
        [HttpPost("Activity")]
        public async Task<IActionResult> ActivityAlgorithm()
        {
            return Ok(activityAlgorithmService.CalculateAllProjects(
                await projectRepository.GetAllWithUsersCollaboratorsAndInstitutionsAsync())
                .OrderByDescending(p => p.ActivityScore).ToList());

        }
    }
}
