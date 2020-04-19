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

        private readonly IMapper mapper;

        private readonly ISearchService searchService;

        /// <summary>
        ///     Initialize a new instance of SearchController
        /// </summary>
        /// <param name="searchService"></param>
        /// <param name="mapper"></param>
        public SearchController(ISearchService searchService, IMapper mapper)
        {
            this.searchService = searchService;
            this.mapper = mapper;
        }

        /// <summary>
        ///     Search for projects
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="parameters"></param>
        /// <returns>Search results</returns>
        [HttpGet("internal/{query}")]
        public async Task<IActionResult> SearchInternalProjects(string query,
                                                                [FromQuery] SearchRequestParamsResource parameters)
        {
            if(query.Length < 0) return BadRequest("Query required");
            if(parameters.Page != null &&
               parameters.Page < 1)
                return BadRequest("Invalid page number");
            if(parameters.SortBy != null &&
               parameters.SortBy != "name" &&
               parameters.SortBy != "created" &&
               parameters.SortBy != "updated")
                return BadRequest("Invalid sort value: Use \"name\", \"created\" or \"updated\"");
            if(parameters.SortDirection != null &&
               parameters.SortDirection != "asc" &&
               parameters.SortDirection != "desc")
                return BadRequest("Invalid sort direction: Use \"asc\" or \"desc\"");

            SearchParams searchParams = mapper.Map<SearchRequestParamsResource, SearchParams>(parameters);
            IEnumerable<Project> projects = await searchService.SearchInternalProjects(query, searchParams);
            IEnumerable<SearchResultResource> searchResults =
                mapper.Map<IEnumerable<Project>, IEnumerable<SearchResultResource>>(projects);

            SearchResultsResource searchResultsResource = new SearchResultsResource()
            {
                Results = searchResults.ToArray(),
                Query = query,
                Count = searchResults.Count(),
                TotalCount = await searchService.SearchInternalProjectsCount(query, searchParams),
                Page = searchParams.Page,
                TotalPages =
                await searchService.SearchInternalProjectsTotalPages(query, searchParams)
            };

            return Ok(searchResultsResource);
        }

    }

}
