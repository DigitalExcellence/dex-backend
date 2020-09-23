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
using System.Threading.Tasks;

namespace API.Controllers
{

    /// <summary>
    ///     Highlight controller for highlights
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HighlightController : ControllerBase
    {

        private readonly IHighlightService highlightService;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightController"/> class.
        /// </summary>
        /// <param name="highlightService">The highlight service.</param>
        /// <param name="mapper">The mapper.</param>
        public HighlightController(IHighlightService highlightService, IMapper mapper)
        {
            this.highlightService = highlightService;
            this.mapper = mapper;
        }

        /// <summary>
        ///     Get all active highlights.
        /// </summary>
        /// <returns>A list of Highlight resource results.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllHighlights()
        {
            IEnumerable<Highlight> highlights = await highlightService.GetHighlightsAsync();
            if(!highlights.Any())
            {
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "Failed getting highlights.",
                    Detail = "The database does not contain any highlights.",
                    Instance = "FC6A4F97-C815-4A92-8A73-2ECF1729B161"
                };
                return NotFound(problem);
            }

            return Ok(mapper.Map<IEnumerable<Highlight>, IEnumerable<HighlightResourceResult>>(highlights));
        }

        /// <summary>
        /// Gets the highlight by the identifier.
        /// </summary>
        /// <param name="highlightId">The highlight identifier.</param>
        /// <returns>A Highlight resource result.</returns>
        [HttpGet("{highlightId}")]
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
        ///     Get a Highlight by project id
        /// </summary>
        /// <param name="projectId">The project identifier to retrieve the corresponding highlights</param>
        /// <returns></returns>
        [HttpGet("Project/{projectId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.HighlightRead))]
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
        ///     Creates a highlight
        /// </summary>
        /// <param name="highlightResource">The highlight resource.</param>
        /// <returns>The created highlight resource result.</returns>
        [HttpPost]
        [Authorize(Policy = nameof(Defaults.Scopes.HighlightWrite))]
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
        /// Updates the highlight.
        /// </summary>
        /// <param name="highlightId">The highlight identifier.</param>
        /// <param name="highlightResource">The highlight resource.</param>
        /// <returns>The updated highlight resource result.</returns>
        [HttpPut("{highlightId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.HighlightWrite))]
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
        /// Deletes the highlight by the identifier.
        /// </summary>
        /// <param name="highlightId">The highlight identifier.</param>
        /// <returns> Status code 200.</returns>
        [HttpDelete("{highlightId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.HighlightWrite))]
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
