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
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Services;
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
    public class SearchController : ControllerBase
    {

        private readonly IMapper _mapper;

        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService, IMapper mapper)
        {
            _searchService = searchService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Search for projects
        /// </summary>
        /// <param name="query">The search query</param>
        /// <returns>Search results</returns>
        [HttpGet("internal/{query}")]
        public async Task<IActionResult> SearchInternalProjects(string query,
                                                                [FromQuery] SearchRequestParamsResource parameters)
        {
            ProblemDetails RequestDetail = new ProblemDetails
            {
                Title = "Invalid search request"
            };
            if(query.Length < 0)
            {
                RequestDetail.Detail = "Query required";
                return BadRequest(RequestDetail);
            }
            if(parameters.Page != null &&
               parameters.Page < 1)
            {
                RequestDetail.Detail = "Invalid page number";
                return BadRequest(RequestDetail);
            }
            if(parameters.SortBy != null &&
               parameters.SortBy != "name" &&
               parameters.SortBy != "created" &&
               parameters.SortBy != "updated")
            {
                RequestDetail.Detail = "Invalid sort value: Use \"name\", \"created\" or \"updated\"";
                return BadRequest(RequestDetail);
            }
            if(parameters.SortDirection != null &&
               parameters.SortDirection != "asc" &&
               parameters.SortDirection != "desc")
            {
                RequestDetail.Detail = "Invalid sort direction: Use \"asc\" or \"desc\"";
                return BadRequest(RequestDetail);
            }

            SearchParams searchParams = _mapper.Map<SearchRequestParamsResource, SearchParams>(parameters);
            IEnumerable<Project> projects = await _searchService.SearchInternalProjects(query, searchParams);
            IEnumerable<SearchResultResource> searchResults =
                _mapper.Map<IEnumerable<Project>, IEnumerable<SearchResultResource>>(projects);

            SearchResultsResource searchResultsResource = new SearchResultsResource();
            searchResultsResource.Results = searchResults.ToArray();
            searchResultsResource.Query = query;
            searchResultsResource.Count = searchResults.Count();
            searchResultsResource.TotalCount = await _searchService.SearchInternalProjectsCount(query, searchParams);
            searchResultsResource.Page = searchParams.Page;
            searchResultsResource.TotalPages =
                await _searchService.SearchInternalProjectsTotalPages(query, searchParams);

            return Ok(searchResultsResource);
        }

    }

}
