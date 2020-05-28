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
using Models;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{

    /// <summary>
    ///     The controller that handles search requests
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WizardController : ControllerBase
    {

        private readonly SourceManagerService sourceManagerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardController"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="sourceManagerService">The source manager service.</param>
        public WizardController(IMapper mapper, SourceManagerService sourceManagerService)
        {
            this.sourceManagerService = sourceManagerService;

        }

        /// <summary>
        /// Gets the wizard information.
        /// </summary>
        /// <param name="sourceURI">The source URI.</param>
        /// <returns></returns>
        [HttpGet("wizard")]
        [Authorize]
        public async Task<IActionResult> GetWizardInformation(Uri sourceURI)
        {

            if(sourceURI == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Source uri is null or empty.",
                    Detail = "the incoming source url is not valid.",
                    Instance = "6D63D9FA-91D6-42D5-9ACB-461FBEB0D2ED"
                };
                return BadRequest(problem);
            }
            Project project = sourceManagerService.FetchProject(sourceURI);
            if(project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
    }
}
