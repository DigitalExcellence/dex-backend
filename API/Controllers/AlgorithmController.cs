using API.Extensions;
using API.InputOutput.ActivityAlgorithm;
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
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlgorithmController" /> class
        /// </summary>
        /// <param name="activityAlgorithmService">The category service which is used to communicate with the logic layer.</param>
        /// <param name="projectRepository">The project category service which is used to communicate with the logic layer.</param>
        public AlgorithmController(IActivityAlgorithmService activityAlgorithmService, IProjectRepository projectRepository, IUserService userService)
        {
            this.activityAlgorithmService = activityAlgorithmService;
            this.projectRepository = projectRepository;
            this.userService = userService;
        }

        /// <summary>
        /// This endpoint initiates the Activity Algorithm.
        /// </summary>
        /// <returns>HttpStatusCode</returns>
        [HttpPost("Activity")]
        [Authorize]
        public async Task<IActionResult> ActivityAlgorithm()
        {
            bool isAllowed = HttpContext.User.HasClaim("client_role", Defaults.Roles.BackendApplication);

            if(isAllowed == false)
            {
                User currentUser = await HttpContext.GetContextUser(userService)
                                                    .ConfigureAwait(false);
                if(currentUser.Role.Name == Defaults.Roles.Administrator)
                {
                    isAllowed = true;
                }
            }
            if(isAllowed)
            {
                return Ok(activityAlgorithmService.CalculateAllProjects(
                                                      await projectRepository.GetAllWithUsersCollaboratorsAndInstitutionsAsync())
                                                  .OrderByDescending(p => p.ActivityScore).ToList());
            }
            return Forbid();

        }

        /// <summary>
        /// 
        /// </summary>
        [HttpPut("UpdateActivityMultiplier")]
        [Authorize]
        public async Task<IActionResult> UpdateActivityMutliplier([FromBody] ActivityAlgorithmInput activityAlgorithmInput)
        {
            bool isAllowed = HttpContext.User.HasClaim("client_role", Defaults.Roles.BackendApplication);

            if(isAllowed == false)
            {
                User currentUser = await HttpContext.GetContextUser(userService)
                                                    .ConfigureAwait(false);
                if(currentUser.Role.Name == Defaults.Roles.Administrator)
                {
                    isAllowed = true;
                }
            }
            if(isAllowed && ModelState.IsValid)
            {
                ActivityAlgorithmMultiplier activityAlgorithmMultiplier = new ActivityAlgorithmMultiplier()
                {
                    Id = 1,
                    AverageLikeDateMultiplier = activityAlgorithmInput.AverageLikeDateMultiplier,
                    ConnectedCollaboratorsMultiplier = activityAlgorithmInput.ConnectedCollaboratorsMultiplier,
                    RecentCreatedDataMultiplier = activityAlgorithmInput.RecentCreatedDataMultiplier,
                    InstitutionMultiplier = activityAlgorithmInput.InstitutionMultiplier,
                    LikeDataMultiplier = activityAlgorithmInput.LikeDataMultiplier,
                    MetaDataMultiplier = activityAlgorithmInput.MetaDataMultiplier,
                    RepoScoreMultiplier = activityAlgorithmInput.RepoScoreMultiplier,
                    UpdatedTimeMultiplier = activityAlgorithmInput.UpdatedTimeMultiplier,
                };
                activityAlgorithmService.SetActivityAlgorithmMultiplier(activityAlgorithmMultiplier);
                return Ok();
            }
            if(ModelState.IsValid)
            {
                return BadRequest();
            }
            return Forbid();
        }
    }
}

