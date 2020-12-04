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
        /// Initializes a new instance of the <see cref="PortfolioController"/> class
        /// </summary>
        /// <param name="mapper">The mapper which is used to convert the resources to the models to the resource results.</param>
        /// <param name="portfolioService">The portfolio service which is used to communicate with the logic layer.</param>
        /// <param name="userService">The user service which is used to communicate with the logic layer.</param>
        /// <param name="authorizationHelper">The authorization helper which is used to communicate with the authorization helper class.</param>
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
                    Detail = "The portfolio id could not be found in the database.",
                    Instance = "FD71D106-17E3-453E-A40D-B39F67D6A517"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<Portfolio, PortfolioResourceResult>(portfolio));
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
        public async Task<IActionResult> CreatePortfolioAsync(PortfolioResource portfolioResource)
        {
            User user = await HttpContext.GetContextUser(userService).ConfigureAwait(false);

            if(await userService.FindAsync(user.Id) == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the user account.",
                    Detail = "The database does not contain a user with this user id.",
                    Instance = "B778C55A-D41E-4101-A7A0-F02F76E5A6AE"
                };
                return NotFound(problem);
            }

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

            try
            {
                portfolio.User = user;
                portfolioService.Add(portfolio);
                portfolioService.Save();
                PortfolioResourceResult model = mapper.Map<Portfolio, PortfolioResourceResult>(portfolio);
                return Created(nameof(CreatePortfolioAsync), model);
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

        /// <summary>
        /// This method is responsible for updating the portfolio with the specified identifier.
        /// </summary>
        /// <param name="portfolioId">The project identifier which is used for searching the project.</param>
        /// <param name="portfolioResource">The project resource which is used for updating the project.</param>
        /// <returns>This method returns the project resource result.</returns>
        /// <response code="200">This endpoint returns the updated project.</response>
        /// <response code="401">The 401 Unauthorized status code is return when the user has not the correct permission to update.</response>
        /// <response code="404">The 404 not Found status code is returned when the project to update is not found.</response>
        [HttpPut("{portfolioId}")]
        [Authorize]
        [ProducesResponseType(typeof(ProjectResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdatePortfolio(int portfolioId, [FromBody] PortfolioResource portfolioResource)
        {
            Portfolio portfolio = await portfolioService.FindAsync(portfolioId).ConfigureAwait(false);
            if(portfolio.Id == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to update portfolio.",
                    Detail = "The specified portfolio could not be found in the database.",
                    Instance = "E0701056-DBA6-4256-8FAC-54A9ADF1284C"
                };
                return NotFound(problem);
            }

            User user = await HttpContext.GetContextUser(userService).ConfigureAwait(false);
            bool isAllowed = userService.UserHasScope(user.IdentityId, nameof(Defaults.Scopes.ProjectWrite));

            if(!(portfolio.User.Id == user.Id || isAllowed))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to edit the portfolio.",
                    Detail = "The user is not allowed to edit the portfolio.",
                    Instance = "43026602-BBFB-44EB-9A8B-AE49B95C0B3E"
                };
                return Unauthorized(problem);
            }

            mapper.Map(portfolioResource, portfolio);
            portfolioService.Update(portfolio);
            portfolioService.Save();
            return Ok(mapper.Map<Portfolio, PortfolioResourceResult>(portfolio));
        }

        /// <summary>
        /// This method is responsible for deleting the portfolio.
        /// </summary>
        /// <param name="portfolioId">The project identifier which is used for searching the project.</param>
        /// <returns>This method returns status code 200.</returns>
        /// <response code="200">This endpoint returns status code 200. The current account is deleted.</response>
        /// <response code="404">The 404 Not Found status code is returned when the current account could not be found.</response>
        [HttpDelete]
        [Authorize]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeletePortfolio(int portfolioId)
        {
            Portfolio portfolio = await portfolioService.FindAsync(portfolioId).ConfigureAwait(false);
            User user = await HttpContext.GetContextUser(userService).ConfigureAwait(false);

            if(portfolio == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to delete portfolio.",
                    Detail = "The specified portfolio could not be found in the database.",
                    Instance = "60329216-3B51-423D-AB8A-BB1F3B55BD4B"
                };
                return NotFound(problem);
            }

            bool isAllowed = userService.UserHasScope(user.IdentityId, nameof(Defaults.Scopes.ProjectWrite));

            if(!(portfolio.User.Id == user.Id || isAllowed))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to delete the portfolio.",
                    Detail = "The user is not allowed to delete the portfolio.",
                    Instance = "D36DEDD5-75DB-4849-92B6-2271D7C350D0"
                };
                return Unauthorized(problem);
            }


            await portfolioService.RemoveAsync(portfolioId);
            portfolioService.Save();
            return Ok();
        }
    }
}
