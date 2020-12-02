using API.Common;
using API.Extensions;
using API.HelperClasses;
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
    /// This class is responsible for handling HTTP requests that are related
    /// to the portfolio and portfolio items, for example creating, retrieving, updating or deleting.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPortfolioService portfolioService;
        private readonly IUserService userService;
        private readonly IAuthorizationHelper authorizationHelper;

        /// <summary>
        /// This method is responsible for retrieving a single portfolio.
        /// </summary>
        /// <param name="portfolioId">the portfolio identifier which is used for searching a portfolio.</param>
        /// <returns>This method returns the user resource result.</returns>
        /// <response code="200">This endpoint returns the portfolio with the specified portfolio id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the portfolio id is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when the portfolio with the specified id could not be found.</response>
        [HttpGet("{portfolioId}")]
        [Authorize]
        [ProducesResponseType(typeof(PortfolioResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPortfolio(int portfolioId)
        {
            User currentUser = await HttpContext.GetContextUser(userService).ConfigureAwait(false);
            bool isAllowed = await authorizationHelper.UserIsAllowed(currentUser,
                                                               nameof(Defaults.Scopes.UserRead),
                                                               nameof(Defaults.Scopes.InstitutionUserRead),
                                                               portfolioId);

            if(!isAllowed)
                return Forbid();

            if(portfolioId < 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the portfolio.",
                    Detail = "The user id is less then zero and therefore cannot exist in the database.",
                    Instance = "EAF7FEA1-47E9-4CF8-8415-4D3BC843FB71",
                };
                return BadRequest(problem);
            }

            Portfolio portfolio = await portfolioService.FindAsync(portfolioId);
            if(portfolio == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the portfolio.",
                    Detail = "The user could not be found in the database.",
                    Instance = "FD71D106-17E3-453E-A40D-B39F67D6A517"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<Portfolio, PortfolioResourceResult>(portfolio));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortfolioController"/> class
        /// </summary>
        /// <param name="userService">The user service which is used to communicate with the logic layer.</param>
        /// <param name="mapper">The mapper which is used to convert the resources to the models to the resource results.</param>
        /// <param name="authorizationHelper">The authorization helper which is used to communicate with the authorization helper class.</param>
        /// <param name="userProjectService">The user project service is responsible for users that are following / liking projects.</param>
        /// <param name="fileUploader">The file uploader service is used to upload the files into the file system</param>
        public PortfolioController(IMapper mapper,
                                   IPortfolioService portfolioService,
                                   IUserService userService,
                                   IAuthorizationHelper authorizationHelper)
        {
            this.mapper = mapper;
            this.portfolioService = portfolioService;
            this.userService = userService;
            this.authorizationHelper = authorizationHelper;
        }

        /// <summary>
        /// This method is responsible for creating a Project.
        /// </summary>
        /// <returns>This method returns the project resource result.</returns>
        /// <response code="200">This endpoint returns the created portfolio.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the portfolio
        /// resource is not specified or failed to save project to the database.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ProjectResourceResult), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePortfolioAsync([FromBody] PortfolioResource portfolioResource)
        {
            if(portfolioResource == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to create a new project.",
                    Detail = "The specified portfolio resource was null.",
                    Instance = "68257E3C-3C75-483D-A35E-BB76FDC6ECAE"
                };
                return BadRequest(problem);
            }
            Portfolio portfolio = mapper.Map<PortfolioResource, Portfolio>(portfolioResource);

            portfolio.User = await HttpContext.GetContextUser(userService).ConfigureAwait(false);

            try
            {
                portfolioService.Add(portfolio);
                portfolioService.Save();
                return Created(nameof(CreatePortfolioAsync), mapper.Map<Portfolio, PortfolioResourceResult>(portfolio));
            } catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception");


                ProblemDetails problem = new ProblemDetails()
                {
                    Title = "Failed to save new portfolio.",
                    Detail = "There was a problem while saving the portfolio to the database.",
                    Instance = "A0FA175B-C339-433E-95C4-F4E0F74EAE3E"
                };
                return BadRequest(problem);
            }
        }
    }
}
