using API.Resources;
using AutoMapper;
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
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialise a new instance of SearchController
        /// </summary>
        public SearchController(ISearchService searchService, 
            IMapper mapper)
        {
            _searchService = searchService;
            _mapper = mapper;
        }

        /// <summary>
        /// Search in internal sources
        /// </summary>
        [HttpGet]
        public IActionResult Internal([FromBody] SearchRequest query)
        {
            IEnumerable<SearchResult> results = _searchService.SearchInternally(query);

            if(results == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<SearchResult>, IEnumerable<SearchResultResource>>(results));
        }

        /// <summary>
        /// Search in external sources
        /// </summary>
        public async Task<IActionResult> External([FromBody] SearchRequest request)
        {
            IEnumerable<SearchResult> results = await _searchService.SearchExternallyAsync(request);

            if (results == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<SearchResult>, IEnumerable<SearchResultResource>>(results));
        }

    }
}