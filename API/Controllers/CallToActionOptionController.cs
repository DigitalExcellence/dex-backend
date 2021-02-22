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
    ///     This class is responsible for handling HTTP requests that are related
    ///     to the call to action options, for example creating, retrieving, updating or deleting.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class CallToActionOptionController : ControllerBase
    {

        private readonly ICallToActionOptionService callToActionOptionService;

        private readonly IMapper mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CallToActionOptionController" /> class.
        /// </summary>
        /// <param name="mapper">The mapper which is used to convert the resources to the models to the resource results.</param>
        /// <param name="callToActionOptionService">
        ///     The call to action option service which is used to communicate with the logic
        ///     layer.
        /// </param>
        public CallToActionOptionController(IMapper mapper, ICallToActionOptionService callToActionOptionService)
        {
            this.mapper = mapper;
            this.callToActionOptionService = callToActionOptionService;
        }

        /// <summary>
        ///     This method is responsible for retrieving all the call to action options.
        /// </summary>
        /// <returns>This method returns a list of call to action option resource results.</returns>
        /// <response code="200">This endpoint returns a list of call to action options.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<CallToActionOptionResourceResult>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllCallToActionOptions()
        {
            IEnumerable<CallToActionOption> options = await callToActionOptionService.GetAll();
            IEnumerable<CallToActionOptionResourceResult> returnModel =
                mapper.Map<IEnumerable<CallToActionOption>, IEnumerable<CallToActionOptionResourceResult>>(options);

            return Ok(returnModel);
        }

        /// <summary>
        ///     This method is responsible for retrieving all the call to action options with
        ///     the specified type name.
        /// </summary>
        /// <param name="typeName">
        ///     The name for the call to action option type which is
        ///     used for searching all the call to action options.
        /// </param>
        /// <returns code="200">This endpoint returns a list of call to action options with the specified type</returns>
        /// <response code="400">The 400 Bad Request status code is returned when the id is invalid.</response>
        /// <response code="404">The 404 Not Found status code is returned when the type could not be found.</response>
        [HttpGet("type/{typeName}")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<CallToActionOptionResourceResult>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllCallToActionOptionsFromType(string typeName)
        {
            if(string.IsNullOrEmpty(typeName))
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Invalid type name specified",
                                             Detail = "The specified type name is invalid.",
                                             Instance = "4C5FE712-E286-43B4-8B2E-6C6BC3985F83"
                                         };
                return BadRequest(problem);
            }

            IEnumerable<CallToActionOption> type =
                await callToActionOptionService.GetCallToActionOptionsFromTypeAsync(typeName.ToLower());
            if(type == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting the call to action option type.",
                                             Detail =
                                                 "The database does not contain a call to action option type with the specified id.",
                                             Instance = "8F83DE66-7CB8-49E4-A204-153C525BCA28"
                                         };
                return NotFound(problem);
            }

            IEnumerable<CallToActionOption> options =
                await callToActionOptionService.GetCallToActionOptionsFromTypeAsync(typeName);
            IEnumerable<CallToActionOptionResourceResult> returnModel =
                mapper.Map<IEnumerable<CallToActionOption>, IEnumerable<CallToActionOptionResourceResult>>(options);

            return Ok(returnModel);
        }

        /// <summary>
        ///     This method is responsible for retrieving a single call to action option by id.
        /// </summary>
        /// <param name="id">The unique identifier which is used for searching the call to action option.</param>
        /// <returns>This method returns the call to action option resource result.</returns>
        /// <response code="200">This endpoint returns a call to action option with the specified id.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the id is invalid.</response>
        /// <response code="404">
        ///     The 404 Not Found status code is returned when no call to action option could be
        ///     found with the specified id.
        /// </response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(CallToActionOptionResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCallToActionOptionById(int id)
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

            CallToActionOption callToActionOption = await callToActionOptionService.FindAsync(id);
            if(callToActionOption == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting the call to action option.",
                                             Detail =
                                                 "The database does not contain a call to action option with the specified id.",
                                             Instance = "1EAFDED6-74B8-4DA4-A58E-375F58AFDB2E"
                                         };
                return NotFound(problem);
            }

            CallToActionOptionResourceResult model =
                mapper.Map<CallToActionOption, CallToActionOptionResourceResult>(callToActionOption);
            return Ok(model);
        }

        /// <summary>
        ///     This method is responsible for creating a call to action option.
        /// </summary>
        /// <param name="callToActionOptionResource">
        ///     The call to action option resource which is used
        ///     to create the call to action option.
        /// </param>
        /// <returns>This method returns the created call to action option resource result</returns>
        /// <response code="201">This endpoint returns the created call to action option.</response>
        /// <response code="400">
        ///     The 400 Bad Request status code is returned when the specified
        ///     resource is invalid or the call to action option could not be saved to the database.
        /// </response>
        [HttpPost]
        [Authorize(Policy = nameof(Defaults.Scopes.CallToActionOptionWrite))]
        [ProducesResponseType(typeof(CallToActionOptionResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateCallToActionOption(CallToActionOptionResource callToActionOptionResource)
        {
            if(callToActionOptionResource == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed creating the call to action option.",
                                             Detail = "The institution resource is null.",
                                             Instance = "E2FD8F7B-96B1-4406-9E90-138AA36B570B"
                                         };
                return BadRequest(problem);
            }

            CallToActionOption option =
                mapper.Map<CallToActionOptionResource, CallToActionOption>(callToActionOptionResource);

            if((await callToActionOptionService.GetCallToActionOptionsFromTypeAsync(option.Type)).Any() &&
               (await callToActionOptionService.GetCallToActionOptionFromValueAsync(option.Value)).Any())
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed creating the call to action option.",
                                             Detail = "Identical call to action option already exists.",
                                             Instance = "1DA7B168-FAD1-41B6-A90F-3AAEB26147CE"
                                         };
                return BadRequest(problem);
            }

            try
            {
                callToActionOptionService.Add(option);
                callToActionOptionService.Save();
                CallToActionOptionResourceResult model =
                    mapper.Map<CallToActionOption, CallToActionOptionResourceResult>(option);
                return Created(nameof(CreateCallToActionOption), model);
            } catch(DbUpdateException)
            {
                Log.Logger.Error("Database exception");

                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed Saving the call to action option.",
                                             Detail = "Failed saving the call to action option to the database.",
                                             Instance = "5A1D2B14-E320-4FAE-84DF-BC02B996588B"
                                         };
                return BadRequest(problem);
            }
        }

        /// <summary>
        ///     This method is responsible for updating the call to action option.
        /// </summary>
        /// <param name="callToActionId">The call to action option identifier which is used to find the call to action option.</param>
        /// <param name="callToActionOptionResource">
        ///     The call to action option resource which is used to update the call to action
        ///     option.
        /// </param>
        /// <returns>This method returns the updated call to action option resource result.</returns>
        /// <response code="200">This endpoint returns the updated call to action option.</response>
        /// <response code="404">
        ///     The 404 Not Found status code is returned when no call to action option is
        ///     found with the specified call to action option id.
        /// </response>
        [HttpPut("{callToActionId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.CallToActionOptionWrite))]
        [ProducesResponseType(typeof(CallToActionOptionResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateCallToActionOption(int callToActionId,
                                                                  [FromBody]
                                                                  CallToActionOptionResource callToActionOptionResource)
        {
            CallToActionOption option = await callToActionOptionService.FindAsync(callToActionId);
            if(option == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting the call to action option.",
                                             Detail =
                                                 "The database does not contain a call to action option with that id.",
                                             Instance = "A939D6FA-4B85-4D3F-B3CC-86658713D76C"
                                         };
                return NotFound(problem);
            }

            mapper.Map(callToActionOptionResource, option);

            if((await callToActionOptionService.GetCallToActionOptionsFromTypeAsync(option.Type)).Any() &&
               (await callToActionOptionService.GetCallToActionOptionFromValueAsync(option.Value)).Any())
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed creating the call to action option.",
                                             Detail = "Identical call to action option already exists.",
                                             Instance = "1DA7B168-FAD1-41B6-A90F-3AAEB26147CE"
                                         };
                return BadRequest(problem);
            }

            callToActionOptionService.Update(option);
            callToActionOptionService.Save();

            return Ok(mapper.Map<CallToActionOption, CallToActionOptionResourceResult>(option));
        }

        /// <summary>
        ///     This method is responsible for deleting the call to action option by the identifier.
        /// </summary>
        /// <param name="id">
        ///     The call to action option identifier which is used to find the
        ///     call to action option.
        /// </param>
        /// <returns>This method returns status code 200.</returns>
        /// <response code="200">This endpoint returns status code 200. The call to action option is deleted.</response>
        /// <response code="404">
        ///     The 404 Not Found status code is returned when no call to action option is found with the
        ///     specified id.
        /// </response>
        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Defaults.Scopes.CallToActionOptionWrite))]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteCallToActionOption(int id)
        {
            CallToActionOption option = await callToActionOptionService.FindAsync(id);
            if(option == null)
            {
                ProblemDetails problem = new ProblemDetails
                                         {
                                             Title = "Failed getting the call to action option.",
                                             Detail =
                                                 "The database does not contain a call to action option with that id.",
                                             Instance = "6BDD3202-AE32-4CC1-AA87-42A2870CE8E6"
                                         };
                return NotFound(problem);
            }

            await callToActionOptionService.RemoveAsync(id);
            callToActionOptionService.Save();
            return Ok();
        }

    }

}
