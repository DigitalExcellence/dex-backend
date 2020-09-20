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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Services;
using System;
using System.Net;

namespace API.Controllers
{
    /// <summary>
    /// The controller that handles search requests
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WizardController : ControllerBase
    {
        private readonly ISourceManagerService sourceManagerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardController"/> class.
        /// </summary>
        /// <param name="sourceManagerService">The source manager service.</param>
        public WizardController(ISourceManagerService sourceManagerService)
        {
            this.sourceManagerService = sourceManagerService;
        }

        /// <summary>
        /// Gets the wizard information.
        /// </summary>
        /// <param name="sourceURI">The source URI.</param>
        /// <returns>The filled in Project.</returns>
        /// <response code="200">Returns the project with the specified source Uri</response>
        /// <response code="400">If source Uri is not specified</response>
        /// <response code="404">If the project could not be found with the specified source Uri</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(Project), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public IActionResult GetWizardInformation(Uri sourceURI)
        {
            if(sourceURI == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Source uri is null or empty.",
                    Detail = "The incoming source uri is not valid.",
                    Instance = "6D63D9FA-91D6-42D5-9ACB-461FBEB0D2ED"
                };
                return BadRequest(problem);
            }
            Project project = sourceManagerService.FetchProject(sourceURI);
            if(project == null)
            {
                return NotFound();
            }
            if(project.Name == null && project.ShortDescription == null && project.Description == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Project not found.",
                    Detail = "The incoming source uri aims at a gitlab which is either not instantiated or is a group.",
                    Instance = "E56D89C5-8760-4503-839C-F695092C79BF"
                };
                return BadRequest(problem);
            }
            return Ok(project);
        }
    }
}
