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
        private readonly IPortfolioItemService portfolioItemService;
        private readonly IProjectService projectService;
        private readonly IUserService userService;
        private readonly IAuthorizationHelper authorizationHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortfolioController"/> class
        /// </summary>
        /// <param name="mapper">The mapper which is used to convert the resources to the models to the resource results.</param>
        /// <param name="portfolioService">The portfolio service which is used to communicate with the logic layer.</param>
        /// <param name="portfolioItemService">The portfolioItem service which is used to communicate with the logic layer.</param>
        /// <param name="projectService">The project service which is used to communicate with the logic layer.</param>
        /// <param name="userService">The user service which is used to communicate with the logic layer.</param>
        /// <param name="authorizationHelper">The authorization helper which is used to communicate with the authorization helper class.</param>
        public PortfolioController(IMapper mapper,
                                   IPortfolioService portfolioService,
                                   IPortfolioItemService portfolioItemService,
                                   IProjectService projectService,
                                   IUserService userService,
                                   IAuthorizationHelper authorizationHelper)
        {
            this.mapper = mapper;
            this.portfolioService = portfolioService;
            this.portfolioItemService = portfolioItemService;
            this.projectService = projectService;
            this.userService = userService;
            this.authorizationHelper = authorizationHelper;
        }

        #region Portfolio

        /// <summary>
        /// This method is responsible for retrieving a single portfolio.
        /// </summary>
        /// <param name="portfolioId">the portfolio identifier which is used for searching a portfolio.</param>
        /// <returns>This method returns the user resource result.</returns>
        /// <response code="200">This endpoint returns the portfolio with the specified portfolio id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the portfolio id is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when the portfolio with the specified id could not be found.</response>
        [HttpGet("{portfolioId}")]
        [ProducesResponseType(typeof(PortfolioResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPortfolio(int portfolioId)
        {
            if(portfolioId <= 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the portfolio.",
                    Detail = "The user id is less then zero and therefore cannot exist in the database.",
                    Instance = "F7669BF7-CE70-47C0-8D29-37516BA887C0",
                };
                return BadRequest(problem);
            }

            Portfolio portfolio = await portfolioService.FindAsync(portfolioId);
            if(portfolio == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "the portfolio doesn't exist.",
                    Detail = "The portfolio id could not be found in the database.",
                    Instance = "91A15229-CD31-4D6E-920E-9DD6889333F1"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<Portfolio, PortfolioResourceResult>(portfolio));
        }

        /// <summary>
        /// This method is responsible for creating a portfolio.
        /// </summary>
        /// <returns>This method returns the portfolio resource result.</returns>
        /// <response code="200">This endpoint returns the created portfolio.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the portfolio
        /// resource is not specified or failed to save portfolio to the database.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(PortfolioResourceResult), (int) HttpStatusCode.Created)]
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
                    Instance = "C199E422-D4BA-49DC-813F-B529959C6AD2"
                };
                return NotFound(problem);
            }

            if(portfolioResource == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to create a new portfolio.",
                    Detail = "The specified portfolio resource was null.",
                    Instance = "0A2C218D-CA78-4384-A6A7-0C8C7C01D56B"
                };
                return BadRequest(problem);
            }

            Portfolio portfolio = mapper.Map<PortfolioResource, Portfolio>(portfolioResource);

            try
            {
                portfolio.User = user;
                await portfolioService.AddAsync(portfolio);
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
                    Instance = "E54E222D-A131-4F78-90AD-266E7CA29DB5"
                };
                return BadRequest(problem);
            }
        }

        /// <summary>
        /// This method is responsible for updating the portfolio with the specified identifier.
        /// </summary>
        /// <param name="portfolioId">The portfolio identifier which is used for searching the portfolio.</param>
        /// <param name="portfolioResource">The portfolio resource which is used for updating the portfolio.</param>
        /// <returns>This method returns the portfolio resource result.</returns>
        /// <response code="200">This endpoint returns the updated portfolio.</response>
        /// <response code="401">The 401 Unauthorized status code is return when the user has not the correct permission to update.</response>
        /// <response code="404">The 404 not Found status code is returned when the portfolio to update is not found.</response>
        [HttpPut("{portfolioId}")]
        [Authorize]
        [ProducesResponseType(typeof(PortfolioResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdatePortfolio(int portfolioId, [FromBody] PortfolioResource portfolioResource)
        {
            User user = await HttpContext.GetContextUser(userService).ConfigureAwait(false);

            Portfolio portfolio = await portfolioService.FindAsync(portfolioId).ConfigureAwait(false);
            if(portfolio == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to update portfolio.",
                    Detail = "The specified portfolio could not be found in the database.",
                    Instance = "65229A71-FDD8-4887-BBCD-3D4258C5D329"
                };
                return NotFound(problem);
            }



            if(!(portfolio.User.Id == user.Id))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to edit the portfolio.",
                    Detail = "The user is not allowed to edit the portfolio.",
                    Instance = "B00CA783-B8E0-4F73-9E19-A74448F1930B"
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
        /// <param name="portfolioId">The portfolio identifier which is used for searching the portfolio.</param>
        /// <returns>This method returns status code 200.</returns>
        /// <response code="200">This endpoint returns status code 200. The current portfolio is deleted.</response>
        /// <response code="404">The 404 Not Found status code is returned when the current portfolio could not be found.</response>
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(typeof(PortfolioItemResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeletePortfolio(int portfolioId)
        {
            Portfolio portfolio = await portfolioService.FindAsync(portfolioId).ConfigureAwait(false);
            User user = await HttpContext.GetContextUser(userService).ConfigureAwait(false);

            if(portfolio.User.Id != user.Id)
                return Forbid();


            if(portfolio == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to delete portfolio.",
                    Detail = "The specified portfolio could not be found in the database.",
                    Instance = "CF5D3E72-6B98-4828-BDBB-E829BDED297D"
                };
                return NotFound(problem);
            }

            if(!(portfolio.User.Id == user.Id))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to delete the portfolio.",
                    Detail = "The user is not allowed to delete the portfolio.",
                    Instance = "2BA2DD8D-EA05-4796-B0C0-C7F3F0CEE5D4"
                };
                return Unauthorized(problem);
            }


            await portfolioService.RemoveAsync(portfolioId);
            portfolioService.Save();
            return Ok();
        }

        #endregion

        #region PortfolioItem

        /// <summary>
        /// This method is responsible for retrieving a single portfolio.
        /// </summary>
        /// <param name="portfolioItemId">the portfolio item identifier which is used for searching a portfolio item.</param>
        /// <returns>This method returns the user resource result.</returns>
        /// <response code="200">This endpoint returns the portfolio with the specified portfolio item id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the portfolio item id is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when the portfolio item with the specified id could not be found.</response>
        [HttpGet("item/{portfolioItemId}")]
        [ProducesResponseType(typeof(PortfolioItemResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPortfolioItem(int portfolioItemId)
        {
            if(portfolioItemId < 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the portfolio item.",
                    Detail = "The user id is less then zero and therefore cannot exist in the database.",
                    Instance = "006B26A3-C8F9-4ABC-BDFD-6A3A6D0D894F",
                };
                return BadRequest(problem);
            }

            PortfolioItem portfolioItem = await portfolioItemService.FindAsync(portfolioItemId);
            if(portfolioItem == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "the portfolio item doesn't exist.",
                    Detail = "The portfolio item id could not be found in the database.",
                    Instance = "A130226D-B26D-4C41-9CCF-C988B6FE3F08"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<PortfolioItem, PortfolioItemResourceResult>(portfolioItem));
        }

        /// <summary>
        /// This method is responsible for creating a portfolio item.
        /// </summary>
        /// <param name="portfolioId">The portfolio identifier which is used for searching the portfolio.</param>
        /// <param name="projectId">The project identifier which is used for searching the project.</param>
        /// <param name="portfolioItemResource">The portfolio item resource which is used for creating the portfolio item.</param>
        /// <returns>This method returns the portfolio item resource result.</returns>
        /// <response code="200">This endpoint returns the created portfolio item.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the portfolio item
        /// resource is not specified or failed to save portfolio item to the database.</response>
        [HttpPost("item")]
        [Authorize]
        [ProducesResponseType(typeof(PortfolioItemResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> CreatePortfolioItemAsync(int portfolioId, int projectId, PortfolioItemResource portfolioItemResource)
            {
            User user = await HttpContext.GetContextUser(userService).ConfigureAwait(false);

            Portfolio portfolio = await portfolioService.FindAsync(portfolioId).ConfigureAwait(false);
            Project project = await projectService.FindAsync(projectId).ConfigureAwait(false);
            PortfolioItem portfolioItem = mapper.Map<PortfolioItemResource, PortfolioItem>(portfolioItemResource);

            if(await userService.FindAsync(user.Id) == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the user account.",
                    Detail = "The database does not contain a user with this user id.",
                    Instance = "C199E422-D4BA-49DC-813F-B529959C6AD2"
                };
                return NotFound(problem);
            }

            if(portfolioItemResource == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to create a new portfolio item.",
                    Detail = "The specified portfolio resource was null.",
                    Instance = "0A2C218D-CA78-4384-A6A7-0C8C7C01D56B"
                };
                return BadRequest(problem);
            }

            if(portfolio == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to get the corresponding portfolio.",
                    Detail = "The specified portfolio resource was null.",
                    Instance = "6C53D257-FBEA-487D-B0B0-DFACCFFAEF9E"
                };
                return BadRequest(problem);
            }

            if(projectId > 0)
            {
                if(project == null)
                {
                    ProblemDetails problem = new ProblemDetails
                    {
                        Title = "Failed to get the corresponding project.",
                        Detail = "The specified project resource was null.",
                        Instance = "34DB000E-BF54-4258-BD70-4F91E64BD92C"
                    };
                    return BadRequest(problem);
                }
            }
            else
            {
                portfolioItem.ProjectId = 0;
            }

            try
            {
                portfolioItem.ProjectId = project.Id;
                portfolioItem.PortfolioId = portfolio.Id;
                portfolioItemService.Add(portfolioItem);
                portfolioItemService.Save();
                PortfolioItemResourceResult model = mapper.Map<PortfolioItem, PortfolioItemResourceResult>(portfolioItem);
                return Created(nameof(CreatePortfolioItemAsync), portfolioItem);
            } catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception");


                ProblemDetails problem = new ProblemDetails()
                {
                    Title = "Failed to save new portfolio item.",
                    Detail = "There was a problem while saving the portfolio item to the database.",
                    Instance = "E54E222D-A131-4F78-90AD-266E7CA29DB5"
                };
                return BadRequest(problem);
            }
        }

        /// <summary>
        /// This method is responsible for updating the portfolio with the specified identifier.
        /// </summary>
        /// <param name="portfolioId">The portfolio identifier which is used for searching the portfolio.</param>
        /// <param name="portfolioItemId">The portfolio item identifier which is used for searching the portfolio item</param>
        /// <param name="portfolioItemResource">The portfolio item resource which is used for updating the portfolio item.</param>
        /// <returns>This method returns the portfolio item resource result.</returns>
        /// <response code="200">This endpoint returns the updated portfolio item.</response>
        /// <response code="401">The 401 Unauthorized status code is return when the user has not the correct permission to update.</response>
        /// <response code="404">The 404 not Found status code is returned when the portfolio to update is not found.</response>
        [HttpPut("item/{portfolioItemId}")]
        [Authorize]
        [ProducesResponseType(typeof(PortfolioItemResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdatePortfolioItem(int portfolioId, int portfolioItemId, [FromBody] PortfolioItemResource portfolioItemResource)
        {
            PortfolioItem portfolioItem = await portfolioItemService.FindAsync(portfolioItemId).ConfigureAwait(false);
            Portfolio portfolio = await portfolioService.FindAsync(portfolioId).ConfigureAwait(false);
            if(portfolioItem == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to update portfolio item.",
                    Detail = "The specified portfolio could not be found in the database.",
                    Instance = "8F2CDC1B-7015-4B72-B91B-78FD839DDEF1"
                };
                return NotFound(problem);
            }

            User user = await HttpContext.GetContextUser(userService).ConfigureAwait(false);

            if(!(portfolio.User.Id == user.Id))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to edit the portfolio.",
                    Detail = "The user is not allowed to edit the portfolio.",
                    Instance = "B00CA783-B8E0-4F73-9E19-A74448F1930B"
                };
                return Unauthorized(problem);
            }

            mapper.Map(portfolioItemResource, portfolioItem);
            portfolioItemService.Update(portfolioItem);
            portfolioItemService.Save();
            return Ok(mapper.Map<PortfolioItem, PortfolioItemResourceResult>(portfolioItem));
        }

        /// <summary>
        /// This method is responsible for deleting the portfolio.
        /// </summary>
        /// <param name="portfolioId">The portfolio identifier which is used for searching the portfolio.</param>
        /// <param name="portfolioItemId">The portfolio Item identifier which is used for searching the portfolio item.</param>
        /// <returns>This method returns status code 200.</returns>
        /// <response code="200">This endpoint returns status code 200. The current portfolio item is deleted.</response>
        /// <response code="404">The 404 Not Found status code is returned when the current portfolio item could not be found.</response>
        [HttpDelete("item/{portfolioItemId}")]
        [Authorize]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeletePortfolioItem(int portfolioId, int portfolioItemId)
        {
            PortfolioItem portfolioItem = await portfolioItemService.FindAsync(portfolioItemId).ConfigureAwait(false);
            Portfolio portfolio = await portfolioService.FindAsync(portfolioId).ConfigureAwait(false);
            User user = await HttpContext.GetContextUser(userService).ConfigureAwait(false);

            if(portfolioItem == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to delete portfolio item.",
                    Detail = "The specified portfolio could not be found in the database.",
                    Instance = "618CCDA0-A8E2-4B8E-B393-661D96A22585"
                };
                return NotFound(problem);
            }

            if(!(portfolio.User.Id == user.Id))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed to delete the portfolio item.",
                    Detail = "The user is not allowed to delete the portfolio.",
                    Instance = "9F0CB10D-0FAE-4112-A91A-79E5C1F798BD"
                };
                return Unauthorized(problem);
            }


            await portfolioItemService.RemoveAsync(portfolioItemId);
            portfolioService.Save();
            return Ok();
        }

    }
    #endregion
}
