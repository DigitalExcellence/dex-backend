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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Services;
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Defaults;

namespace API.Controllers
{
    /// <summary>
    /// The controller that handles search requests
    /// </summary>
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
