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
using Models.Defaults;
using Services.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// This class is responsible for handling HTTP requests that are related
    /// to the call to action options, for example creating, retrieving, updating or deleting.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public partial class CallToActionOptionController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly ICallToActionOptionService callToActionOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallToActionOptionController"/> class.
        /// </summary>
        /// <param name="mapper">The mapper which is used to convert the resources to the models to the resource results.</param>
        /// <param name="callToActionOptionService">The call to action option service which is used to communicate with the logic layer.</param>
        public CallToActionOptionController(IMapper mapper, ICallToActionOptionService callToActionOptionService)
        {
            this.mapper = mapper;
            this.callToActionOptionService = callToActionOptionService;
        }

        /// <summary>
        /// This method is responsible for retrieving all the call to action options.
        /// </summary>
        /// <returns>This method returns a list of call to action option resource results.</returns>
        /// <response code="200">This endpoint returns a list of call to action options.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CallToActionOptionResourceResult>), (int) HttpStatusCode.OK)]
        [Authorize(Policy = nameof(Defaults.Scopes.CallToActionOptionRead))]
        public async Task<IActionResult> GetAllOptions()
        {
            IEnumerable<CallToActionOption> options = await callToActionOptionService.GetCallToActionOptionsAsync();
            IEnumerable<CallToActionOptionResourceResult> returnModel =
                mapper.Map<IEnumerable<CallToActionOption>, IEnumerable<CallToActionOptionResourceResult>>(options);

            return Ok(returnModel);
        }

        /// <summary>
        /// This method is responsible for retrieving all the call to action options with
        /// the specified type id.
        /// </summary>
        /// <param name="id">The unique identifier for the call to action option type which is
        /// used for searching all the call to action options.</param>
        /// <returns code="200">This endpoint returns a list of call to action options with the specified type</returns>
        /// <response code="400">The 400 Bad Request status code is returned when the id is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when the type could not be found.</response>
        [HttpGet("type/{id}")]
        [ProducesResponseType(typeof(IEnumerable<CallToActionOptionResourceResult>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        [Authorize(Policy = nameof(Defaults.Scopes.CallToActionOptionRead))]
        public async Task<IActionResult> GetAllOptionsFromType(int id)
        {
            if(id <= 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Invalid Id specified",
                    Detail = "The specified id is invalid.",
                    Instance = "4C5FE712-E286-43B4-8B2E-6C6BC3985F83"
                };
                return BadRequest(problem);
            }

            //TODO: Check if type exists

            IEnumerable<CallToActionOption> options =
                await callToActionOptionService.GetCallToActionOptionsFromTypeAsync(id);
            IEnumerable<CallToActionOptionResourceResult> returnModel =
                mapper.Map<IEnumerable<CallToActionOption>, IEnumerable<CallToActionOptionResourceResult>>(options);

            return Ok(returnModel);
        }

        /// <summary>
        /// This method is responsible for retrieving a single call to action option by id.
        /// </summary>
        /// <param name="id">The unique identifier which is used for searching the call to action option.</param>
        /// <returns>This method returns the call to action option resource result.</returns>
        /// <response code="200">This endpoint returns a call to action option with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the id is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when no call to action option could be
        /// found with the specified id.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CallToActionOptionResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        [Authorize(Policy = nameof(Defaults.Scopes.CallToActionOptionRead))]
        public async Task<IActionResult> GetOptionById(int id)
        {
            if(id <= 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Invalid Id specified",
                    Detail = "The specified id is invalid.",
                    Instance = "72702E9D-5D99-40F7-A921-033A79275877"
                };
                return BadRequest(problem);
            }

            CallToActionOption callToActionOption = await callToActionOptionService.GetCallToActionOptionByIdAsync(id);
            if(callToActionOption == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the call to action option.",
                    Detail = "The database does not contain a call to action option with the specified id.",
                    Instance = "1EAFDED6-74B8-4DA4-A58E-375F58AFDB2E"
                };
                return NotFound(problem);
            }

            CallToActionOptionResourceResult model =
                mapper.Map<CallToActionOption, CallToActionOptionResourceResult>(callToActionOption);
            return Ok(model);
        }

    }
}
