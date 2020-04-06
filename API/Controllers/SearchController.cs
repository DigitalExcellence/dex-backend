using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Search;
using Services.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Models;
using Sources;
using Services.Sources;

namespace API.Controllers
{
    /// <summary>
    /// Controller for all search endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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
        /// Search in external sources
        /// </summary>
        [HttpGet("external")]
        public async Task<IActionResult> External()
        {
            List<ISource> sources = new List<ISource>();
            sources.Add(new GitLabSource());

            List<SearchQueryParameter> queryParameters = new List<SearchQueryParameter>();
            SearchRequest request = new SearchRequest()
            {
                Sources = sources,
                QueryParameters = queryParameters

            };
            IEnumerable<SearchResult> results = await _searchService.SearchExternallyAsync(request);

            if (results == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<SearchResult>, IEnumerable<SearchResultResource>>(results));
        }
        
        /// <summary>
        /// Search for projects
        /// </summary>
        /// <param name="query">The search query</param>
        /// <returns>Search results</returns>
        [HttpGet("internal/{query}")]
        public async Task<IActionResult> SearchInternalProjects(string query, [FromQuery(Name = "page")] int? page, [FromQuery(Name = "amountOnPage")] int? amountOnPage)
        {

            if (query.Length < 0) return BadRequest("Query required");
            if (page != null && page < 1) return BadRequest("Invalid page number");
            
            amountOnPage ??= 20;
            amountOnPage = amountOnPage <= 0 ? 20 : amountOnPage;

            IEnumerable<Project> projects = page == null ?
                await _searchService.SearchInternalProjects(query) :
                await _searchService.SearchInternalProjectsSkipTake(query, (int) (amountOnPage*(page-1)), (int) amountOnPage);

            List<SearchResultResource> searchResults = new List<SearchResultResource>();
            foreach (Project project in projects)
            {
                SearchResultResource searchResult = _mapper.Map<Project, SearchResultResource>(project);
                searchResults.Add(searchResult);
            }
            SearchResultsResource searchResultsResource = new SearchResultsResource();
            searchResultsResource.Results = searchResults.ToArray();
            searchResultsResource.Query = query;
            searchResultsResource.Count = searchResults.Count();
            searchResultsResource.TotalCount = await _searchService.SearchInternalProjectsCount(query);
            searchResultsResource.Page = page;
            searchResultsResource.TotalPages = (int) Math.Ceiling(searchResultsResource.TotalCount / (decimal) amountOnPage);
            
            return Ok(searchResultsResource);
        }
    }
}