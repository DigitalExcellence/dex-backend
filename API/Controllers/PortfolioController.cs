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
    /// to the portfolio, for example creating, retrieving, updating or deleting.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPortfolioService portfolioService;
        private readonly IFileUploader fileUploader;
        private readonly IFileService fileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortfolioController"/> class
        /// </summary>
        /// <param name="portfolioService">The service which is used to communciate with the logic layer.</param>
        /// <param name="mapper">The mapper which is used to communicate with the logic layer.</param>
        /// <param name="fileUploader">The file uploader service is used to upload the files into the file system</param>
        /// <param name="fileService">The file service which is used to communicate with the logic layer.</param>
        /// 
        public PortfolioController(
            IPortfolioService portfolioService,
            IMapper mapper,
            IFileUploader fileUploader,
            IFileService fileService)
        {
            this.mapper = mapper;
            this.portfolioService = portfolioService;
            this.fileUploader = fileUploader;
            this.fileService = fileService;
        }

        /// <summary>
        /// This method is responsible for retrieving a single portfolio.
        /// </summary>
        /// <returns>This method returns the project resource result.</returns>
        /// <response code="200">This endpoint returns the project with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified id is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when the project could not be found with the specified id.</response>
        [HttpGet("{portfolioId}")]
        [ProducesResponseType(typeof(ProjectResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPortfolio(int portfolioId)
        {
            if(portfolioId < 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting portfolio.",
                    Detail = "The Id is smaller then 0 and therefore it could never be a valid portfolio id.",
                    Instance = "F7014218-03C3-4B1B-8E4A-EA08826DF69E"
                };
                return BadRequest(problem);
            }

            Portfolio portfolio = await portfolioService.FindAsync(portfolioId);
            if(portfolio == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting portfolio.",
                    Detail = "The portfolio could not be found in the database.",
                    Instance = "BD8D48EE-39F4-4BD8-A042-2ECA0FC956F0"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<Portfolio, PortfolioResourceResult>(portfolio));
        }

        /// <summary>
        /// This method is responsible for creating a portfolio.
        /// </summary>
        /// <returns>This method returns the project resource result.</returns>
        /// <response code="200">This endpoint returns the created project.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the project
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
                    Title = "Failed to create a new portfolio.",
                    Detail = "The specified project resource was null.",
                    Instance = "1507D4F9-5326-4E44-BDC0-39CA4248050F"
                };
                return BadRequest(problem);
            }
            Portfolio portfolio = mapper.Map<PortfolioResource, Portfolio>(portfolioResource);

            try
            {
                portfolioService.Add(portfolio);
                portfolioService.Save();
                return Created(nameof(CreatePortfolioAsync), mapper.Map<PortfolioResource, Portfolio>(portfolioResource));
            } catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception");


                ProblemDetails problem = new ProblemDetails()
                {
                    Title = "Failed to save new portfolio.",
                    Detail = "There was a problem while saving the project to the database.",
                    Instance = "FA6E8774-9A12-4E61-8FCF-DF81819FEB3C"
                };
                return BadRequest(problem);
            }
        }
    }
}
