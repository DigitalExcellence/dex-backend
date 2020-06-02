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
        /// <param name="projectFilterParamsResource">The parameters to filter, sort and paginate the projects</param>
        /// <returns>Search results</returns>
        [HttpGet("internal/{query}")]
        public async Task<IActionResult> SearchInternalProjects(string query,
                                                                [FromQuery] ProjectFilterParamsResource projectFilterParamsResource)
        {
            ProblemDetails problem = new ProblemDetails
            {
                Title = "Invalid search request."
            };
            if(query.Length < 0)
            {
                problem.Detail = "The Query parameter cannot be empty.";
                problem.Instance = "13A59FAE-E98F-42B3-AFD4-84F3019EC790";
                return BadRequest(problem);
            }
            if(projectFilterParamsResource.Page != null &&
               projectFilterParamsResource.Page < 1)
            {
                problem.Detail = "The page number cannot be smaller then 1.";
                problem.Instance = "65EB6EF1-2CF4-4F7B-8A0A-C047C701337A";
                return BadRequest(problem);
            }
            if(projectFilterParamsResource.SortBy != null &&
               projectFilterParamsResource.SortBy != "name" &&
               projectFilterParamsResource.SortBy != "created" &&
               projectFilterParamsResource.SortBy != "updated")
            {
                problem.Detail = "Invalid sort value: Use \"name\", \"created\" or \"updated\".";
                problem.Instance = "5CE2F569-C0D5-4179-9299-62916270A058";
                return BadRequest(problem);
            }
            if(projectFilterParamsResource.SortDirection != null &&
               projectFilterParamsResource.SortDirection != "asc" &&
               projectFilterParamsResource.SortDirection != "desc")
            {
                problem.Detail = "Invalid sort direction: Use \"asc\" or \"desc\".";
                problem.Instance = "3EE043D5-070B-443A-A951-B252A1BB8EF9";
                return BadRequest(problem);
            }

            ProjectFilterParams projectFilterParams = mapper.Map<ProjectFilterParamsResource, ProjectFilterParams>(projectFilterParamsResource);
            IEnumerable<Project> projects = await searchService.SearchInternalProjects(query, projectFilterParams);
            IEnumerable<ProjectResultResource> searchResults =
                mapper.Map<IEnumerable<Project>, IEnumerable<ProjectResultResource>>(projects);

            ProjectResultsResource searchResultsResource = new ProjectResultsResource()
            {
                Results = searchResults.ToArray(),
                Query = query,
                Count = searchResults.Count(),
                TotalCount = await searchService.SearchInternalProjectsCount(query, projectFilterParams),
                Page = projectFilterParams.Page,
                TotalPages =
                await searchService.SearchInternalProjectsTotalPages(query, projectFilterParams)
            };

            return Ok(searchResultsResource);
        }

    }

}
