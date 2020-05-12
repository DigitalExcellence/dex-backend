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
using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Services
{

    public interface ISearchService
    {

        Task<IEnumerable<Project>> SearchInternalProjects(string query, SearchParams searchParams);

        Task<int> SearchInternalProjectsCount(string query, SearchParams searchParams);

        Task<int> SearchInternalProjectsTotalPages(string query, SearchParams searchParams);

    }

    public class SearchService : ISearchService
    {

        private readonly IProjectRepository projectRepository;

        public SearchService(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        /// <summary>
        ///     Search for projects in the internal database
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="searchParams">The parameters to narrow down the search results</param>
        /// <returns>The projects that match the search query</returns>
        public virtual async Task<IEnumerable<Project>> SearchInternalProjects(string query, SearchParams searchParams)
        {
            if(!searchParams.AmountOnPage.HasValue ||
               searchParams.AmountOnPage <= 0)
                searchParams.AmountOnPage = 20;

            int? skip = null;
            int? take = null;
            if(searchParams.Page.HasValue)
            {
                skip = searchParams.AmountOnPage * (searchParams.Page - 1);
                take = searchParams.AmountOnPage;
            }

            Expression<Func<Project, object>> orderBy;
            switch(searchParams.SortBy)
            {
                case "name":
                    orderBy = project => project.Name;
                    break;
                case "created":
                    orderBy = project => project.Created;
                    break;
                default:
                    orderBy = project => project.Updated;
                    break;
            }

            bool orderByDirection = searchParams.SortDirection == "asc";

            return await projectRepository.SearchAsync(query, skip, take, orderBy, orderByDirection, searchParams.Highlighted);
        }

        /// <summary>
        ///     Get the number of projects that match the search query
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="searchParams">The parameters to narrow down the search results</param>
        /// <returns>The number of projects that match the search query</returns>
        public virtual async Task<int> SearchInternalProjectsCount(string query, SearchParams searchParams)
        {
            return await projectRepository.SearchCountAsync(query);
        }

        /// <summary>
        ///     Search for projects in the internal database
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="searchParams">The parameters to narrow down the search results</param>
        /// <returns>The projects that match the search query</returns>
        public virtual async Task<int> SearchInternalProjectsTotalPages(string query, SearchParams searchParams)
        {
            if(searchParams.AmountOnPage == null ||
               searchParams.AmountOnPage <= 0)
                searchParams.AmountOnPage = 20;
            int count = await SearchInternalProjectsCount(query, searchParams);
            return (int) Math.Ceiling(count / (decimal) searchParams.AmountOnPage);
        }

    }

}
