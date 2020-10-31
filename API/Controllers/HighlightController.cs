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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// This class is responsible for handling HTTP requests that are related
    /// to the highlights, for example creating, retrieving, updating or deleting.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HighlightController : ControllerBase
    {
        private readonly IHighlightService highlightService;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightController"/> class
        /// </summary>
        /// <param name="highlightService">The highlight service which is used to communicate with the logic layer.</param>
        /// <param name="mapper">The mapper which is used to convert the resources to the models to resource results.</param>
        public HighlightController(IHighlightService highlightService, IMapper mapper)
        {
            this.highlightService = highlightService;
            this.mapper = mapper;
        }

        /// <summary>
        /// This method is responsible for retrieving all active highlights.
        /// </summary>
        /// <returns>This method returns a list of highlight resource results.</returns>
        /// <response code="200">This endpoint returns a list highlights.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HighlightResourceResult>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllHighlights()
        {
            IEnumerable<Highlight> highlights = await highlightService.GetHighlightsAsync();

            return Ok(mapper.Map<IEnumerable<Highlight>, IEnumerable<HighlightResourceResult>>(highlights));
        }

        /// <summary>
        /// This method is responsible for retrieving a single highlight by the identifier.
        /// </summary>
        /// <param name="highlightId">The highlight identifier which is used to find the highlight.</param>
        /// <returns>This method returns a highlight resource result.</returns>
        /// <response code="200">This endpoint returns the highlight with the specified identifier.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the highlight id is not valid.</response>
        /// <response code="404">The 404 Not Found status code is returned when there is no highlight found with the specified id.</response>
        [HttpGet("{highlightId}")]
        [ProducesResponseType(typeof(HighlightResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetHighlight(int highlightId)
        {
            if(highlightId < 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting highlight.",
                    Detail = "the highlight id cannot be smaller then 0.",
                    Instance = "BBC86ABA-142B-4BEB-801B-03100C08500B"
                };
                return BadRequest(problem);
            }

            Highlight highlight = await highlightService.FindAsync(highlightId);
            if(highlight == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting highlight.",
                    Detail = "The database does not contain a highlight with this id.",
                    Instance = "1EA0824A-D017-4BAB-9606-2872487D1EDA"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<Highlight, HighlightResourceResult>(highlight));
        }

        /// <summary>
        /// This method is responsible for retrieving all highlight by project id.
        /// </summary>
        /// <param name="projectId">The project identifier which is used to retrieve the corresponding highlights.</param>
        /// <returns></returns>
        /// <response code="200">This endpoint returns a list of highlights from a project.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified project id is not valid.</response>
        /// <response code="404">The 404 Not Found status code is return when there are no highlight found with the specified project id.</response>
        [HttpGet("Project/{projectId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.HighlightRead))]
        [ProducesResponseType(typeof(IEnumerable<HighlightResourceResult>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetHighlightsByProjectId(int projectId)
        {
            if(projectId < 0)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting highlights.",
                    Detail = "The project id cannot be smaller than 0.",
                    Instance = "744F5E01-FC84-4D4A-9A73-0D7C48886A30"
                };
                return BadRequest(problem);
            }
            IEnumerable<Highlight> highlights = await highlightService.GetHighlightsByProjectIdAsync(projectId);
            if(!highlights.Any())
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting highlights.",
                    Detail = "The database does not contain highlights with this project id.",
                    Instance = "D8D040F1-7B29-40AF-910B-D1B1CE809ADC"
                };
                return NotFound(problem);
            }
            return Ok(mapper.Map<IEnumerable<Highlight>, IEnumerable<HighlightResourceResult>>(highlights));
        }

        /// <summary>
        /// This method is responsible for creating a highlight.
        /// </summary>
        /// <param name="highlightResource">The highlight resource which is used to create the highlight.</param>
        /// <returns>This method returns the created highlight resource result.</returns>
        /// <response code="201">This endpoint returns the created highlight.</response>
        /// <response code="400">The 400 Bad Request status code is returned when the specified
        /// resource is invalid or the highlight could not be saved to the database.</response>
        [HttpPost]
        [Authorize(Policy = nameof(Defaults.Scopes.HighlightWrite))]
        [ProducesResponseType(typeof(HighlightResourceResult), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public IActionResult CreateHighlight(HighlightResource highlightResource)
        {
            if(highlightResource == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed creating highlight.",
                    Detail = "The highlight resource is null.",
                    Instance = "2206D5EC-9E20-44A7-A729-0205BEA994E5"
                };
                return BadRequest(problem);
            }

            Highlight highlight = mapper.Map<HighlightResource, Highlight>(highlightResource);

            try
            {
                highlightService.Add(highlight);
                highlightService.Save();
                return Created(nameof(CreateHighlight), mapper.Map<Highlight, HighlightResourceResult>(highlight));
            } catch(DbUpdateException e)
            {
                Log.Logger.Error(e, "Database exception");

                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed Saving highlight.",
                    Detail = "Failed saving the highlight to the database.",
                    Instance = "764E41C3-06A5-47D6-9642-C9E8A0B7CFC7"
                };
                return BadRequest(problem);
            }
        }

        /// <summary>
        /// This method is responsible for updating the highlight.
        /// </summary>
        /// <param name="highlightId">The highlight identifier which is used to find the highlight.</param>
        /// <param name="highlightResource">The highlight resource which is used to update the highlight.</param>
        /// <returns>This method return the updated highlight resource result</returns>
        /// <response code="200">This endpoint returns the updated highlight.</response>
        /// <response code="404">The 404 Not Found status code is returned when no highlight is found with the specified highlight id.</response>
        [HttpPut("{highlightId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.HighlightWrite))]
        [ProducesResponseType(typeof(HighlightResourceResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateHighlight(int highlightId,
                                                         [FromBody] HighlightResource highlightResource)
        {
            Highlight highlight = await highlightService.FindAsync(highlightId);
            if(highlight == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting the highlight.",
                    Detail = "The database does not contain a highlight with that id.",
                    Instance = "795CDB7E-DB46-4A24-8F12-7557BDD79D15"
                };
                return NotFound(problem);
            }

            mapper.Map(highlightResource, highlight);

            highlightService.Update(highlight);
            highlightService.Save();

            return Ok(mapper.Map<Highlight, HighlightResourceResult>(highlight));
        }

        /// <summary>
        /// This method is responsible for deleting the highlight by the identifier.
        /// </summary>
        /// <param name="highlightId">The highlight identifier which is used to find the highlight.</param>
        /// <returns>This method returns status code 200.</returns>
        /// <response code="200">This endpoint returns status code 200. The highlight is deleted.</response>
        /// <response code="404">The 404 Not Found status code is returned when no highlight is found with the specified id.</response>
        [HttpDelete("{highlightId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.HighlightWrite))]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteHighlight(int highlightId)
        {
            if(await highlightService.FindAsync(highlightId) == null)
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting highlight.",
                    Detail = "The database does not contain a highlight with that id.",
                    Instance = "E5B9B140-8B1C-434A-913B-4DB460342BE1"
                };
                return NotFound(problem);
            }
            await highlightService.RemoveAsync(highlightId);
            highlightService.Save();
            return Ok();
        }
    }
}
