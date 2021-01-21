/*
* Digital Excellence Copyright (C) 2020 Brend Smits
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as published
* by the Free Software Foundation version 3 of the License.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU Lesser General Public License for more details.
*
* You can find a copy of the GNU Lesser General Public License
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.DataProviders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// This class is responsible for handling HTTP requests that are related
    /// to the wizard, for exampling retrieving.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WizardController : ControllerBase
    {
        private readonly IDataProviderService dataProviderService;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardController"/> class.
        /// </summary>
        /// <param name="dataProviderService">The source manager service which is used to communicate with the logic layer.</param>
        /// <param name="mapper">The mapper which is used to convert the resources to the models to the resource results.</param>
        public WizardController(
            IDataProviderService dataProviderService,
            IMapper mapper)
        {
            this.dataProviderService = dataProviderService;
            this.mapper = mapper;
        }

        /// <summary>
        /// This method is responsible for retrieving a project form an external data source by
        /// the specified source uri.
        /// </summary>
        /// <param name="dataSourceGuid">The guid that specifies the data source.</param>
        /// <param name="sourceUri">The uri that specifies which project will get retrieved.</param>
        /// <returns>This method returns the found project with the specified source uri.</returns>
        /// <response code="200">This endpoint returns the project with the specified source uri.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the source uri is empty
        /// or whenever the data source guid is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when no data source is found
        /// with the specified data source guid or no project is found with the specified source uri.</response>
        [HttpGet("project/uri/{sourceUri}")]
        [Authorize]
        [ProducesResponseType(typeof(WizardProjectResourceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectByUriFromExternalDataSource(
            [FromQuery] string dataSourceGuid,
            string sourceUri)
        {
            if(sourceUri == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Source uri is null or empty.",
                    Detail = "The incoming source uri is not valid.",
                    Instance = "6D63D9FA-91D6-42D5-9ACB-461FBEB0D2ED"
                };
                return BadRequest(problem);
            }

            if(!Guid.TryParse(dataSourceGuid, out Guid _))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Specified guid is not valid.",
                    Detail = "The specified guid is not a real or valid guid.",
                    Instance = "9FAF4C56-5B09-46C2-9A52-902D82ADAFA6"
                };
                return BadRequest(problem);
            }

            if(!dataProviderService.IsExistingDataSourceGuid(dataSourceGuid))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Data source not found.",
                    Detail = "Data source could not be found with specified data source guid.",
                    Instance = "DA33EB64-13EF-46CC-B3E6-785E4027377A"
                };
                return NotFound(problem);
            }

            Project project = await dataProviderService.GetProjectFromUri(dataSourceGuid, sourceUri);
            if(project == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Project could not be found.",
                    Detail = "The project could not be found with the specified source Uri and data source guid",
                    Instance = "993252E8-61C4-422D-A547-EB9F56BA47B7"
                };
                return NotFound(problem);
            }
            return Ok(mapper.Map<Project, WizardProjectResourceResult>(project));
        }

        /// <summary>
        /// This method is responsible for retrieving projects from an external data source.
        /// </summary>
        /// <param name="dataSourceGuid">The guid that specifies the data source.</param>
        /// <param name="token">The token which is used for retrieving the projects from the user.</param>
        /// <param name="needsAuth">The bool that represents whether the flow with authorization should get used.</param>
        /// <returns>This method returns a collection of all the projects.</returns>
        /// <response code="200">This endpoint returns the project with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified data source guid is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when no data source is found with the specified data source guid.</response>
        [HttpGet("projects")]
        [Authorize]
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectsFromExternalDataSource(
            [FromQuery] string dataSourceGuid,
            [FromQuery] string token,
            [FromQuery] bool needsAuth)
        {
            if(!Guid.TryParse(dataSourceGuid, out Guid _))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Specified guid is not valid.",
                    Detail = "The specified guid is not a real or valid guid.",
                    Instance = "D84D3112-855D-480A-BCDE-7CADAC2C6C55"
                };
                return BadRequest(problem);
            }

            if(!dataProviderService.IsExistingDataSourceGuid(dataSourceGuid))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Data source not found.",
                    Detail = "Data source could not be found with specified data source guid.",
                    Instance = "4FB90F9A-8499-40F1-B7F3-3C2838BDB1D4"
                };
                return NotFound(problem);
            }

            IEnumerable<Project> projects = await dataProviderService.GetAllProjects(dataSourceGuid, token, needsAuth);
            return Ok(mapper.Map<IEnumerable<Project>, IEnumerable<WizardProjectResourceResult>>(projects));
        }

        /// <summary>
        /// This method is responsible for retrieving a specified project from an external data source.
        /// </summary>
        /// <param name="dataSourceGuid">The guid that specifies the data source.</param>
        /// <param name="accessToken">The access token which is used for authentication.</param>
        /// <param name="projectId">The id of the project which is used for searching a specific project.</param>
        /// <param name="needsAuth">The bool that represents whether the flow with authorization should get used.</param>
        /// <returns>This method returns the project data source resource result</returns>
        /// <response code="200">This endpoint returns the project with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified data source guid is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when no data source is found with the specified data source guid.</response>
        [HttpGet("project/{projectId}")]
        [Authorize]
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectByGuidFromExternalDataSource([FromQuery] string dataSourceGuid,
                                                                    [FromQuery] string accessToken,
                                                                    int projectId,
                                                                    [FromQuery] bool needsAuth)
        {
            if(!Guid.TryParse(dataSourceGuid, out Guid _))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Specified guid is not valid.",
                    Detail = "The specified guid is not a real or valid guid.",
                    Instance = "019146D8-4162-43DD-8531-57DDD26E221C"
                };
                return BadRequest(problem);
            }

            if(dataProviderService.IsExistingDataSourceGuid(dataSourceGuid))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Data source not found.",
                    Detail = "Data source could not be found with specified data source guid.",
                    Instance = "4E3837F4-9D35-40C4-AB7C-D325FBA225E6"
                };
                return NotFound(problem);
            }

            Project project = await dataProviderService.GetProjectByGuid(dataSourceGuid, accessToken, projectId, needsAuth);

            if(project == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Project not found",
                    Detail = "Project could not be found with specified project guid",
                    Instance = "0D96A77A-D35F-487C-B552-BF6D1C0CDD42"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<Project, WizardProjectResourceResult>(project));
        }
    }
}
