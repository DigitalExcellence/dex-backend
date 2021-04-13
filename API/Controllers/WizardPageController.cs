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
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Defaults;
using Serilog;
using Services.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{

    /// <summary>
    ///     This class is responsible for handling HTTP requests that are related to
    ///     the wizard page, for example creating, retrieving, updating and deleting.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WizardPageController : ControllerBase
    {

        private readonly IMapper mapper;

        private readonly IWizardPageService wizardPageService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPageController" /> class.
        /// </summary>
        /// <param name="mapper">The mapper which is used to convert the resources to the models to the resource results.</param>
        /// <param name="wizardPageService">The wizard page service which is used to communicate with the logic layer.</param>
        public WizardPageController(IMapper mapper, IWizardPageService wizardPageService)
        {
            this.mapper = mapper;
            this.wizardPageService = wizardPageService;
        }

        /// <summary>
        ///     This method is responsible for retrieving all the wizard pages stored in the database.
        /// </summary>
        /// <returns>This method returns a collection of wizard pages.</returns>
        /// <response code="200">This endpoint returns the collection of wizard pages.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<WizardPageResourceResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPages()
        {
            IEnumerable<WizardPage> pages = await wizardPageService.GetAll();
            IEnumerable<WizardPageResourceResult> models =
                mapper.Map<IEnumerable<WizardPage>, IEnumerable<WizardPageResourceResult>>(pages);
            return Ok(models);
        }

        /// <summary>
        ///     This method is responsible for retrieving a wizard page by the specified id.
        /// </summary>
        /// <param name="id">The id which is used for searching the project wizard page.</param>
        /// <returns>This method returns the wizard page with the specified id.</returns>
        /// <response code="200">This endpoint returns a wizard page with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the id is not specified.</response>
        /// <response code="404">
        ///     The 404 Not Found status code is returned when no wizard page could be
        ///     found with the specified id.
        /// </response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(WizardPageResourceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPageById(int id)
        {
            if(id <= 0)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Invalid id specified.",
                                             Detail = "The specified id is invalid.",
                                             Instance = "C204ED32-70A4-498D-9EB2-A73EF69F4DA0"
                                         };
                return BadRequest(problem);
            }

            WizardPage page = await wizardPageService.FindAsync(id);
            if(page == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting the wizard page.",
                                             Detail =
                                                 "The database does not contain a wizard page with the specified id.",
                                             Instance = "E562B217-5847-4429-B61B-FAF1F8B33975"
                                         };
                return NotFound(problem);
            }

            WizardPageResourceResult model = mapper.Map<WizardPage, WizardPageResourceResult>(page);
            return Ok(model);
        }

        /// <summary>
        ///     This method is responsible for creating a wizard page.
        /// </summary>
        /// <param name="wizardPageResource">The wizard page resource is used for creating the wizard page.</param>
        /// <returns>This method returns the created wizard page.</returns>
        /// <response code="200">This endpoint returns the created wizard page.</response>
        /// <response code="400">
        ///     The 400 Bad Request status code is returned when the id is invalid or
        ///     when a database update exception occured.
        /// </response>
        [HttpPost]
        [Authorize(Policy = nameof(Defaults.Scopes.WizardPageWrite))]
        [ProducesResponseType(typeof(WizardPageResourceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateWizardPage(WizardPageResource wizardPageResource)
        {
            if(wizardPageResource == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed creating the wizard page.",
                                             Detail = "The wizard page resource is null.",
                                             Instance = "9EDA7F00-BA1E-4DD2-808D-093853FC5534"
                                         };
                return BadRequest(problem);
            }

            WizardPage wizardPage = mapper.Map<WizardPageResource, WizardPage>(wizardPageResource);

            try
            {
                await wizardPageService.AddAsync(wizardPage);
                wizardPageService.Save();
                WizardPageResourceResult createdPage = mapper.Map<WizardPage, WizardPageResourceResult>(wizardPage);
                return Created(nameof(CreateWizardPage), createdPage);
            } catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception");

                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed saving wizard page.",
                                             Detail = "Failed saving the wizard page to the database.",
                                             Instance = "09BCA75E-7615-4E30-9379-61A0C9DC05B8"
                                         };
                return BadRequest(problem);
            }
        }

        /// <summary>
        ///     This method is responsible for updating a wizard page.
        /// </summary>
        /// <param name="wizardPageResource">The wizard page resource is used for updating the wizard page.</param>
        /// <param name="id">The id is used for searching the wizard page that will get updated.</param>
        /// <returns>This method returns the updated wizard page.</returns>
        /// <response code="200">This endpoint returns the updated wizard page.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the id is invalid.</response>
        /// <response code="404">
        ///     The 404 Not Found status code is returned when no wizard page could could get found with
        ///     the specified id.
        /// </response>
        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Defaults.Scopes.WizardPageWrite))]
        [ProducesResponseType(typeof(WizardPageResourceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatedWizardPage(int id, [FromBody] WizardPageResource wizardPageResource)
        {
            if(id <= 0)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Invalid id specified.",
                                             Detail = "The specified id is invalid.",
                                             Instance = "EC827999-28A5-42EF-A160-F8729F26DB13"
                                         };
                return BadRequest(problem);
            }

            WizardPage wizardPage = await wizardPageService.FindAsync(id);
            if(wizardPage == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting the wizard page.",
                                             Detail =
                                                 "The database does not contain a wizard page with the specified id.",
                                             Instance = "ED11431F-AE28-43EE-A1E4-780354217A1E"
                                         };
                return NotFound(problem);
            }

            mapper.Map(wizardPageResource, wizardPage);

            wizardPageService.Update(wizardPage);
            wizardPageService.Save();

            WizardPageResourceResult model = mapper.Map<WizardPage, WizardPageResourceResult>(wizardPage);
            return Ok(model);
        }

        /// <summary>
        ///     This method is responsible for deleting a wizard page.
        /// </summary>
        /// <param name="id">The id is used for searching the wizard page that will get deleted.</param>
        /// <returns>This method returns status code 200 Ok. The wizard page is deleted.</returns>
        /// <response code="200">This endpoint returns status cod 200 Ok. The wizard page is deleted.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the id is invalid.</response>
        /// <response code="404">
        ///     The 404 Not Found status code is returned when no wizard page could could get found with
        ///     the specified id.
        /// </response>
        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Defaults.Scopes.WizardPageWrite))]
        [ProducesResponseType(typeof(WizardPageResourceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteWizardPage(int id)
        {
            if(id <= 0)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Invalid id specified.",
                                             Detail = "The specified id is invalid.",
                                             Instance = "CB6F045F-0F2A-4988-B1F2-3B6B1E8F34AD"
                                         };
                return BadRequest(problem);
            }
            WizardPage wizardPage = await wizardPageService.FindAsync(id);
            if(wizardPage == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting the wizard page.",
                                             Detail =
                                                 "The database does not contain a wizard page with the specified id.",
                                             Instance = "E225D99D-34DB-4B4C-99B5-D6E0722A1F4F"
                                         };
                return NotFound(problem);
            }

            await wizardPageService.RemoveAsync(id);
            wizardPageService.Save();
            return Ok();
        }

    }

}
