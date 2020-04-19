using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Defaults;
using Services.Services;

namespace API.Controllers
{
    /// <summary>
    /// Highlight controller for highlights
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HighlightController : ControllerBase
    {
        private readonly IHighlightService highlightService;
        private readonly IMapper mapper;

        /// <summary>
        /// Initialize a new instance of HighlightController
        /// </summary>
        /// <param name="highlightService"></param>
        /// <param name="mapper"></param>
        public HighlightController(IHighlightService highlightService, IMapper mapper)
        {
            this.highlightService = highlightService;
            this.mapper = mapper;
        }
        /// <summary>
        /// Get all highlights.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllHighlights(bool returnOnlyTheHightlighted = true)
        {
            IEnumerable<Highlight> highlights = await highlightService.GetHighlightsAsync(returnOnlyTheHightlighted);
            if(!highlights.Any())
            {
                return NotFound();
            }

            return Ok(mapper.Map<IEnumerable<Highlight>, IEnumerable<HighlightResourceResult>>(highlights));
        }

        /// <summary>
        /// Get a Highlight by id
        /// </summary>
        /// <param name="highlightId"></param>
        /// <returns></returns>
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetHighlight(int highlightId)
        {
            if(highlightId < 0)
            {
                return BadRequest("Invalid highlight Id");
            }

            Highlight highlight = await highlightService.FindAsync(highlightId);
            if(highlight == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Highlight, HighlightResourceResult>(highlight));
        }

        /// <summary>
        /// Creates a highlight
        /// </summary>
        /// <param name="highlightResource"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = nameof(Defaults.Scopes.HighlightWrite))]
        public async Task<IActionResult> CreateHighlightAsync(HighlightResource highlightResource)
        {
            if(highlightResource == null)
            {
                return BadRequest("Highlight is null");
            }
            Highlight highlight = mapper.Map<HighlightResource, Highlight>(highlightResource);
            try
            {
                highlightService.Add(highlight);
                highlightService.Save();
                return Created(nameof(CreateHighlightAsync), mapper.Map<Highlight, HighlightResourceResult>(highlight));
            } catch
            {
                return BadRequest("Could not Create the Highlight");
            }
        }

        /// <summary>
        /// Update the Highlight
        /// </summary>
        /// <param name="highlightId"></param>
        /// <param name="highlightResource"></param>
        /// <returns></returns>
        [HttpPut("{highlightId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.HighlightWrite))]
        public async Task<IActionResult> UpdateHighlight(int highlightId, [FromBody]HighlightResource highlightResource)
        {
            Highlight highlight = await highlightService.FindAsync(highlightId);
            if(highlight == null)
            {
                return NotFound();
            }

            mapper.Map<HighlightResource, Highlight>(highlightResource, highlight);

            highlightService.Update(highlight);
            highlightService.Save();

            return Ok(mapper.Map<Highlight, HighlightResourceResult>(highlight));
        }


        /// <summary>
        /// Removes a highlight by id
        /// </summary>
        /// <param name="highlightId"></param>
        /// <returns></returns>
        [HttpDelete("{highlightId}")]
        [Authorize(Policy = nameof(Defaults.Scopes.HighlightWrite))]
        public async Task<IActionResult> DeleteHighlight(int highlightId)
        {
            if(await highlightService.FindAsync(highlightId) == null)
            {
                return NotFound();
            }
            await highlightService.RemoveAsync(highlightId);
            highlightService.Save();
            return Ok();
        }
    }
}
