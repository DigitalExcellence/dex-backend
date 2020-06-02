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
using Services.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Services
{

    public interface IProjectService : IService<Project>
    {

        Task<List<Project>> GetAllWithUsersAsync(Params parameters);

        Task<Project> FindWithUserAndCollaboratorsAsync(int id);

        Task<int> ProjectsCount(Params searchParams);

        Task<int> GetProjectsTotalPages(Params searchParams);

    }

    public class ProjectService : Service<Project>, IProjectService
    {

        public ProjectService(IProjectRepository repository) : base(repository) { }

        protected new IProjectRepository Repository => (IProjectRepository) base.Repository;

        /// <summary>
        /// Get a list of all the projects
        /// </summary>
        /// <param name="parameters">The parameters to narrow down the results</param>
        /// <returns>A list of all the projects</returns>
        public Task<List<Project>> GetAllWithUsersAsync(Params parameters)
        {
            if(!parameters.AmountOnPage.HasValue ||
               parameters.AmountOnPage <= 0)
                parameters.AmountOnPage = 20;

            int? skip = null;
            int? take = null;
            if(parameters.Page.HasValue)
            {
                skip = parameters.AmountOnPage * (parameters.Page - 1);
                take = parameters.AmountOnPage;
            }

            Expression<Func<Project, object>> orderBy;
            switch(parameters.SortBy)
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

            bool orderByDirection = parameters.SortDirection == "asc";
            return Repository.GetAllWithUsersAsync(skip, take, orderBy, orderByDirection, parameters.Highlighted);
        }

        /// <summary>
        ///     Get the number of projects
        /// </summary>
        /// <param name="parameters">The parameters to narrow down the results</param>
        /// <returns>The number of projects</returns>
        public virtual async Task<int> ProjectsCount(Params parameters)
        {
            return await Repository.CountAsync(parameters.Highlighted);
        }

        /// <summary>
        ///     Get the total number of pages for the results
        /// </summary>
        /// <param name="parameters">The parameters to narrow down the results</param>
        /// <returns>The total number of pages for the results</returns>
        public virtual async Task<int> GetProjectsTotalPages(Params parameters)
        {
            if(parameters.AmountOnPage == null ||
               parameters.AmountOnPage <= 0)
                parameters.AmountOnPage = 20;
            int count = await ProjectsCount(parameters);
            return (int) Math.Ceiling(count / (decimal) parameters.AmountOnPage);
        }

        public Task<Project> FindWithUserAndCollaboratorsAsync(int id)
        {
            return Repository.FindWithUserAndCollaboratorsAsync(id);
        }

    }

}
