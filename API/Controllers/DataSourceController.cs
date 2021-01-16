using API.HelperClasses;
using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Defaults;
using Services.DataProviders;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    ///
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceController"/> class.
        /// </summary>
        /// <param name="mapper">The mapper which is used to convert the resources to the models to the resource results.</param>
        /// <param name="dataSourceModelService">The data source model service which is used to communicate with the logic layer.</param>
        /// <param name="fileService">The file service which is used to communicate with the logic layer.</param>
        /// <param name="fileUploader">The file uploader service which is used for uploading files.</param>
        /// <param name="dataProviderService">The data provider service which is used to communicate with the logic layer.</param>
        public DataSourceController(IMapper mapper,
                                    IDataSourceModelService dataSourceModelService,
                                    IFileService fileService,
                                    IFileUploader fileUploader,
                                    IDataProviderService dataProviderService)
        {
            this.mapper = mapper;
            this.dataSourceModelService = dataSourceModelService;
            this.fileService = fileService;
            this.fileUploader = fileUploader;
            this.dataProviderService = dataProviderService;
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
                    Title = "No data source with the specified guid fount.",
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

            mapper.Map(dataSourceResource, dataSourceModel);

            dataSourceModelService.Update(dataSourceModel);
            dataSourceModelService.Save();

            return Ok(mapper.Map<DataSource, DataSourceResourceResult>(dataSourceModel));
        }
    }
}
