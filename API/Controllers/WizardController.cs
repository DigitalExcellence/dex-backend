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
using Models.Exceptions;
using Services.ExternalDataProviders;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace API.Controllers
{

    /// <summary>
    ///     This class is responsible for handling HTTP requests that are related
    ///     to the wizard, for exampling retrieving.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WizardController : ControllerBase
    {

        private readonly IDataProviderService dataProviderService;
        private readonly IMapper mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardController" /> class.
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
        ///     This method is responsible for retrieving a project from an external data source by
        ///     the specified source uri.
        /// </summary>
        /// <param name="dataSourceGuid">The guid that specifies the data source.</param>
        /// <param name="sourceUri">The uri that specifies which project will get retrieved.</param>
        /// <returns>This method returns the found project with the specified source uri.</returns>
        /// <response code="200">This endpoint returns the project with the specified source uri.</response>
        /// <response code="400">
        ///     The 400 Bad Request status code is returned when the source uri is empty
        ///     or whenever the data source guid is invalid.
        /// </response>
        /// <response code="404">
        ///     The 404 Not Found status code is returned when no data source is found
        ///     with the specified data source guid or no project is found with the specified source uri.
        /// </response>
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

            try
            {
                Project project = await dataProviderService.GetProjectFromUri(dataSourceGuid, sourceUri);
                if(project == null)
                {
                    ProblemDetails problem = new ProblemDetails
                                             {
                                                 Title = "Project could not be found.",
                                                 Detail =
                                                     "The project could not be found with the specified source Uri and data source guid.",
                                                 Instance = "993252E8-61C4-422D-A547-EB9F56BA47B7"
                                             };
                    return NotFound(problem);
                }
                return Ok(mapper.Map<Project, WizardProjectResourceResult>(project));
            } catch(NotSupportedByExternalApiException e)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "External API does not support the functionality from the method.",
                                             Detail = e.Message,
                                             Instance = "DD815174-8711-4EF0-B01B-776709EDF485"
                                         };
                return BadRequest(problem);
            } catch(ExternalException e)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "An problem encountered while using the external API.",
                                             Detail = e.Message,
                                             Instance = "AA4FC30F-85F0-4120-A479-728DADABAB32"
                                         };
                return BadRequest(problem);
            } catch(NotSupportedException e)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "The specified data source is not supported.",
                                             Detail = e.Message,
                                             Instance = "E7834AC0-43D0-4D40-AB7C-E120A6EFCD5B"
                                         };
                return BadRequest(problem);
            }
        }

        /// <summary>
        ///     This method is responsible for retrieving projects from an external data source.
        /// </summary>
        /// <param name="dataSourceGuid">The guid that specifies the data source.</param>
        /// <param name="token">
        ///     The token which is used for retrieving the projects from the user. This token can be the
        ///     access token for the auth flow, but can also be the username for the public flow.
        /// </param>
        /// <param name="needsAuth">The bool that represents whether the flow with authorization should get used.</param>
        /// <returns>This method returns a collection of all the projects.</returns>
        /// <response code="200">This endpoint returns the project with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified data source guid is invalid.</response>
        /// <response code="404">
        ///     The 404 Not Found status code is returned when no data source is found with the specified data
        ///     source guid.
        /// </response>
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

            try
            {
                IEnumerable<Project> projects =
                    await dataProviderService.GetAllProjects(dataSourceGuid, token, needsAuth);
                return Ok(mapper.Map<IEnumerable<Project>, IEnumerable<WizardProjectResourceResult>>(projects));
            } catch(NotSupportedByExternalApiException e)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "External API does not support the functionality from the method.",
                                             Detail = e.Message,
                                             Instance = "8492B945-7C09-425B-9D1D-77869CE67146"
                                         };
                return BadRequest(problem);
            } catch(ExternalException e)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "An problem encountered while using the external API.",
                                             Detail = e.Message,
                                             Instance = "DE7A6BFD-2A72-46CC-AB6E-1D8568F2EB19"
                                         };
                return BadRequest(problem);
            } catch(NotSupportedException e)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "The specified data source is not supported.",
                                             Detail = e.Message,
                                             Instance = "E1500627-AAF8-46E3-9B20-8A3C952CDBC3"
                                         };
                return BadRequest(problem);
            }
        }

        /// <summary>
        ///     This method is responsible for retrieving a specified project from an external data source.
        /// </summary>
        /// <param name="dataSourceGuid">The guid that specifies the data source.</param>
        /// <param name="token">
        ///     The token which is used for retrieving the projects from the user. This token can be the
        ///     access token for the auth flow, but can also be the username for the public flow.
        /// </param>
        /// <param name="projectId">The id of the project which is used for searching a specific project.</param>
        /// <param name="needsAuth">The bool that represents whether the flow with authorization should get used.</param>
        /// <returns>This method returns the project data source resource result</returns>
        /// <response code="200">This endpoint returns the project with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified data source guid is invalid.</response>
        /// <response code="404">
        ///     The 404 Not Found status code is returned when no data source is found with the specified data
        ///     source guid.
        /// </response>
        [HttpGet("project/{projectId}")]
        [Authorize]
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectByGuidFromExternalDataSource([FromQuery] string dataSourceGuid,
            [FromQuery] string token,
            string projectId,
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

            if(!dataProviderService.IsExistingDataSourceGuid(dataSourceGuid))
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Data source not found.",
                                             Detail = "Data source could not be found with specified data source guid.",
                                             Instance = "4E3837F4-9D35-40C4-AB7C-D325FBA225E6"
                                         };
                return NotFound(problem);
            }

            try
            {
                Project project =
                    await dataProviderService.GetProjectById(dataSourceGuid, token, projectId, needsAuth);

                if(project == null)
                {
                    ProblemDetails problem = new ProblemDetails
                                             {
                                                 Title = "Project not found.",
                                                 Detail = "Project could not be found with specified project guid.",
                                                 Instance = "0D96A77A-D35F-487C-B552-BF6D1C0CDD42"
                                             };
                    return NotFound(problem);
                }

                return Ok(mapper.Map<Project, WizardProjectResourceResult>(project));
            } catch(NotSupportedByExternalApiException e)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "External API does not support the functionality from the method.",
                                             Detail = e.Message,
                                             Instance = "F20B0D1F-D6B7-4BCE-9BC8-28B9E9618214"
                                         };
                return BadRequest(problem);
            } catch(ExternalException e)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "An problem encountered while using the external API.",
                                             Detail = e.Message,
                                             Instance = "21D8A923-02CB-4F1B-86C0-88FDA002294D"
                                         };
                return BadRequest(problem);
            } catch(NotSupportedException e)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "The specified data source is not supported.",
                                             Detail = e.Message,
                                             Instance = "0D02B0F5-71F8-427E-AB28-D4831B91639D"
                                         };
                return BadRequest(problem);
            }
        }

        /// <summary>
        ///     This method is responsible for converting the code from the data provider to the correct tokens.
        /// </summary>
        /// <param name="provider">The guid or name that specifies the data source.</param>
        /// <param name="code">The access token which is used for authentication.</param>
        /// <returns>This method returns the project data source resource result</returns>
        /// <response code="200">This endpoint returns the project with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified data source guid is invalid.</response>
        /// <response code="404">
        ///     The 404 Not Found status code is returned when no data source is found with the specified data
        ///     source guid.
        /// </response>
        [HttpGet("oauth/callback/{provider}")]
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DataProviderCallback(string provider, string code)
        {
            IDataSourceAdaptee dataSourceAdaptee;
            if(Guid.TryParse(provider, out Guid _))
            {
                dataSourceAdaptee = await dataProviderService.RetrieveDataSourceByGuid(provider);
            } else
            {
                dataSourceAdaptee = await dataProviderService.RetrieveDataSourceByName(provider);
            }

            if(dataSourceAdaptee == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Data source not found.",
                                             Detail = "Data source could not be found with specified data source guid.",
                                             Instance = "5B4E11A6-8209-4F49-B76A-1EF4297D990F"
                                         };
                return NotFound(problem);
            }

            IPrivateDataSourceAdaptee privateDataSourceAdaptee =
                dataSourceAdaptee as IPrivateDataSourceAdaptee;

            if(privateDataSourceAdaptee == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "The specified provider does not allowed authorization.",
                                             Detail = "The specified provider is not able to verify the code.",
                                             Instance = "EB1F47B2-5526-41F3-8C69-8068F12A92D1"
                                         };
                return BadRequest(problem);
            }

            try
            {
                OauthTokens tokens = await privateDataSourceAdaptee.GetTokens(code);
                OauthTokensResourceResult resourceResult = mapper.Map<OauthTokens, OauthTokensResourceResult>(tokens);
                return Ok(resourceResult);
            } catch(NotSupportedByExternalApiException e)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "External API does not support the functionality from the method.",
                                             Detail = e.Message,
                                             Instance = "CDFFA448-38B1-450F-8D14-FB89FB7B5462"
                                         };
                return BadRequest(problem);
            } catch(ExternalException e)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "An problem encountered while using the external API.",
                                             Detail = e.Message,
                                             Instance = "7D445CB5-7C19-449C-B9FF-4214E4BE4CF0"
                                         };
                return BadRequest(problem);
            } catch(NotSupportedException e)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "The specified data source is not supported.",
                                             Detail = e.Message,
                                             Instance = "7F2C173E-F001-49CA-8DF8-C18A0837B4AF"
                                         };
                return BadRequest(problem);
            }
        }

    }

}
