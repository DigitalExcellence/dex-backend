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

using API.HelperClasses;
using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Defaults;
using Services.ExternalDataProviders;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// This class is responsible for handling HTTP requests that are related to the data sources, for example retrieving and updating.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DataSourceController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly IDataSourceModelService dataSourceModelService;
        private readonly IFileService fileService;
        private readonly IFileUploader fileUploader;
        private readonly IDataProviderService dataProviderService;
        private readonly IIndexOrderHelper<int> indexOrderHelper;
        private readonly IWizardPageService wizardPageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceController"/> class.
        /// </summary>
        /// <param name="mapper">The mapper which is used to convert the resources to the models to the resource results.</param>
        /// <param name="dataSourceModelService">The data source model service which is used to communicate with the logic layer.</param>
        /// <param name="fileService">The file service which is used to communicate with the logic layer.</param>
        /// <param name="fileUploader">The file uploader service which is used for uploading files.</param>
        /// <param name="dataProviderService">The data provider service which is used to communicate with the logic layer.</param>
        /// <param name="indexOrderHelper">The index order helper, helps validating the index order of the wizard pages.</param>
        /// <param name="wizardPageService">The wizard page service which is used to communicate with the logic layer.</param>
        public DataSourceController(IMapper mapper,
                                    IDataSourceModelService dataSourceModelService,
                                    IFileService fileService,
                                    IFileUploader fileUploader,
                                    IDataProviderService dataProviderService,
                                    IIndexOrderHelper<int> indexOrderHelper,
                                    IWizardPageService wizardPageService)
        {
            this.mapper = mapper;
            this.dataSourceModelService = dataSourceModelService;
            this.fileService = fileService;
            this.fileUploader = fileUploader;
            this.dataProviderService = dataProviderService;
            this.indexOrderHelper = indexOrderHelper;
            this.wizardPageService = wizardPageService;
        }


        /// <summary>
        /// This method is responsible for retrieving data sources.
        /// </summary>
        /// <param name="needsAuth">This parameter specifies whether the data sources should need authentication.</param>
        /// <returns>This method returns a collection of data sources.</returns>
        /// <response code="200">This endpoint returns the available data sources with the specified flow.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<DataSourceResourceResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAvailableDataSources([FromQuery] bool? needsAuth)
        {
            IEnumerable<IDataSourceAdaptee> dataSources = await dataProviderService.RetrieveDataSources(needsAuth);
            IEnumerable<DataSourceResourceResult> dataSourceResourceResult =
                mapper.Map<IEnumerable<IDataSourceAdaptee>, IEnumerable<DataSourceResourceResult>>(dataSources);
            return Ok(dataSourceResourceResult);
        }

        /// <summary>
        /// This method is responsible for retrieving a data source by guid.
        /// </summary>
        /// <param name="guid">The guid is used for searching the data source with this specified guid.</param>
        /// <returns>This method returns a data source with the specified guid.</returns>
        /// <response code="200">This endpoint returns the found data source with the specified guid.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified guid is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when no data source with the specified
        /// guid could be found.</response>
        [HttpGet("guid")]
        [Authorize]
        [ProducesResponseType(typeof(DataSourceResourceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDataSourceById(string guid)
        {
            if(!Guid.TryParse(guid, out Guid _))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Specified guid is not valid.",
                    Detail = "The specified guid is not a real or valid guid.",
                    Instance = "64052C41-FB93-4733-918F-056C765044AE"
                };
                return BadRequest(problem);
            }

            IDataSourceAdaptee dataSource = await dataProviderService.RetrieveDataSourceByGuid(guid);

            if(dataSource == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "No data source with the specified guid found.",
                    Detail = "The database does not contain an institution with that guid.",
                    Instance = "3B2C12E3-CDE0-4853-A687-C1024E096479"
                };
                return NotFound(problem);
            }

            DataSourceResourceResult dataSourceResourceResult =
                mapper.Map<IDataSourceAdaptee, DataSourceResourceResult>(dataSource);
            return Ok(dataSourceResourceResult);
        }

        /// <summary>
        /// This method is responsible for updating the data source in the database.
        /// </summary>
        /// <param name="guid">The guid parameter is used for searching the data source that should get updated.</param>
        /// <param name="dataSourceResource">The data source resource contains the new data that gets used
        /// for updating the specified data source.</param>
        /// <returns>This method returns the updated data source resource result.</returns>
        /// <response code="200">This endpoint returns the updated data source.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified data source guid is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when no data source is found with the specified data source guid
        /// or whenever no file is found with the specified id.</response>
        [HttpPut("{guid}")]
        [Authorize(Policy = nameof(Defaults.Scopes.DataSourceWrite))]
        [ProducesResponseType(typeof(DataSourceResourceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDataSource(string guid, [FromBody] DataSourceResource dataSourceResource)
        {
            if(!Guid.TryParse(guid, out Guid _))
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Specified guid is not valid.",
                    Detail = "The specified guid is not a real or valid guid.",
                    Instance = "F472CEEC-BBC7-41A7-87C9-24B669DB9D80"
                };
                return BadRequest(problem);
            }

            DataSource dataSourceModel = await dataSourceModelService.GetDataSourceByGuid(guid);

            if(dataSourceModel == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed retrieving the data source.",
                    Detail = "The database does not contain an institution with that guid.",
                    Instance = "031FE0E3-D8CF-4DEC-81D5-E89B33BED8D0"
                };
                return NotFound(problem);
            }

            DataSource dataSourceWithSpecifiedName =
                await dataSourceModelService.GetDataSourceByName(dataSourceResource.Title);

            if(dataSourceWithSpecifiedName != null && dataSourceWithSpecifiedName.Guid != guid)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Specified name of the data source already exists",
                    Detail =
                        "Another data source already has the specified name, no doubles are allowed",
                    Instance = "804F134C-E679-4AF5-B602-18433F26019A"
                };
                return BadRequest(problem);
            }

            if(!await wizardPageService.ValidateWizardPagesExist(
                    dataSourceResource.WizardPageResources.Select(w => w.WizardPageId)))
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Not all specified wizard pages could be found.",
                                             Detail = "One or more specified wizard page ids don't exist",
                                             Instance ="EF1490B0-DB22-4A0D-B6D1-D6E89192381E"
                                         };
                return NotFound(problem);
            }

            if(dataSourceResource.IconId != 0)
            {
                if(dataSourceModel.Icon != null)
                {
                    File fileToDelete = await fileService.FindAsync(dataSourceModel.Icon.Id);
                    fileUploader.DeleteFileFromDirectory(fileToDelete);
                    await fileService.RemoveAsync(dataSourceModel.Icon.Id);
                    fileService.Save();
                }

                File file = await fileService.FindAsync(dataSourceResource.IconId);
                if(file != null)
                {
                    dataSourceModel.Icon = file;
                } else
                {
                    ProblemDetails problem = new ProblemDetails
                    {
                        Title = "File was not found.",
                        Detail = "The specified file was not found while updating project.",
                        Instance = "7A6BF2DE-A0BC-4C84-8CC4-89EC0C706EAB"
                    };
                    return NotFound(problem);
                }
            }

            int[] wizardPageOrderIndexesAuthFlow = dataSourceResource.WizardPageResources?.Where(p => p.AuthFlow).Select(p => p.OrderIndex).ToArray();
            int[] wizardPageOrderIndexesPublicFlow = dataSourceResource.WizardPageResources?.Where(p => !p.AuthFlow).Select(p => p.OrderIndex).ToArray();

            bool authFlowIsValid = wizardPageOrderIndexesAuthFlow?.Length == 0 ||
                                   indexOrderHelper.ValidateAscendingConsecutiveOrder(wizardPageOrderIndexesAuthFlow, 1);

            bool publicFlowIsValid = wizardPageOrderIndexesPublicFlow?.Length == 0 ||
                                     indexOrderHelper.ValidateAscendingConsecutiveOrder(wizardPageOrderIndexesPublicFlow, 1);

            if(!authFlowIsValid || !publicFlowIsValid)
            {
                ProblemDetails problem = new ProblemDetails
                    {
                        Title = "The order from the wizard page indexes is invalid.",
                        Detail = "The order indexes from the wizard pages should start at 1, be consecutive and have no doubles.",
                        Instance = "A5F70346-8044-42AC-8BFD-76FCD108ABBE"
                    };
                return BadRequest(problem);
            }

            mapper.Map(dataSourceResource, dataSourceModel);

            dataSourceModelService.Update(dataSourceModel);
            dataSourceModelService.Save();

            DataSource updatedDataSourceModel = await dataSourceModelService.GetDataSourceByGuid(guid);
            DataSourceResourceResult model = mapper.Map<DataSource, DataSourceResourceResult>(updatedDataSourceModel);
            return Ok(model);
        }
    }
}
