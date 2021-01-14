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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// This class is responsible for handling HTTP requests that are related to
    /// the wizard page, for example creating, retrieving, updating and deleting.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WizardPageController : ControllerBase
    {

        private readonly IMapper mapper;

        private readonly IWizardPageService wizardPageService;

        public WizardPageController(IMapper mapper, IWizardPageService wizardPageService)
        {
            this.mapper = mapper;
            this.wizardPageService = wizardPageService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllPages()
        {
            IEnumerable<WizardPage> pages = await wizardPageService.GetAll();
            IEnumerable<WizardPageResourceResult> models = mapper.Map<IEnumerable<WizardPage>, IEnumerable<WizardPageResourceResult>>(pages);
            return Ok(models);
        }

        [HttpGet("{id}")]
        [Authorize]
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
                    Detail = "The database does not contain a wizard page with the specified id.",
                    Instance = "E562B217-5847-4429-B61B-FAF1F8B33975"
                };
                return NotFound(problem);
            }

            WizardPageResourceResult model = mapper.Map<WizardPage, WizardPageResourceResult>(page);
            return Ok(model);
        }

        [HttpPost]
        [Authorize]
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
                wizardPageService.Add(wizardPage);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatedWizardPage(int id, [FromBody] WizardPageResource wizardPageResource)
        {
            WizardPage wizardPage = await wizardPageService.FindAsync(id);
            if(wizardPage == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the wizard page.",
                    Detail = "The database does not contain a wizard page with the specified id.",
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWizardPage(int id)
        {
            WizardPage wizardPage = wizardPageService.FindAsync(id);
            if(wizardPage == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the wizard page.",
                    Detail = "The database does not contain a wizard page with the specified id.",
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
