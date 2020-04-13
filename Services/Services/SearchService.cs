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
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Repositories;

namespace Services.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<Project>> SearchInternalProjects(string query);
        
        Task<IEnumerable<Project>> SearchInternalProjectsSkipTake(string query, int skip, int take);
        
        Task<int> SearchInternalProjectsCount(string query);
        
    }

    public class SearchService : ISearchService
    {
        private IProjectRepository _projectRepository;
        
        public SearchService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public virtual async Task<IEnumerable<Project>> SearchInternalProjects(string query)
        {
            return await _projectRepository.SearchAsync(query);
        }
        
        // Search for projects
        // @param query The search query
        // @param skip The amount of results to skip
        // @param take The amount of results to return
        public virtual async Task<IEnumerable<Project>> SearchInternalProjectsSkipTake(string query, int skip, int take)
        {
            return await _projectRepository.SearchSkipTakeAsync(query, skip, take);
        }
        
        public virtual async Task<int> SearchInternalProjectsCount(string query)
        {
            return await _projectRepository.SearchCountAsync(query);
        }
        
    }
}