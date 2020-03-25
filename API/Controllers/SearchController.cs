using Microsoft.AspNetCore.Mvc;
using Search;
using Services.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// Controller for all search endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        /// <summary>
        /// Initialise a new instance of SearchController
        /// </summary>
        /// <param name="searchService"></param>
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        /// <summary>
        /// Search in internal sources
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Internal([FromBody] SearchRequest query)
        {
            IEnumerable<SearchResult> results = await _searchService.SearchInternallyAsync(query);

            if(results == null)
            {
                return NoContent();
            }

            return Ok(results);
        }

        /// <summary>
        /// Search in external sources
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IActionResult> External([FromBody] SearchRequest request)
        {
            IEnumerable<SearchResult> results = await _searchService.SearchExternallyAsync(request);

            if (results == null)
            {
                return NoContent();
            }

            return Ok(results);
        }

    }
}