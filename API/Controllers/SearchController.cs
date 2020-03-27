using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        
        private readonly ISearchService _searchService;
        private readonly IMapper _mapper;
        
        public SearchController(ISearchService searchService, IMapper mapper)
        {
            _searchService = searchService;
            _mapper = mapper;
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
            if (page < 1) return BadRequest("Invalid page number");
            
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