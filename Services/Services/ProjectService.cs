﻿/*
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

        /// <summary>
        /// Get a list of all the projects
        /// </summary>
        /// <param name="projectFilterParams">The parameters to filter, sort and paginate the projects</param>
        /// <returns>A list of all the projects</returns>
        Task<List<Project>> GetAllWithUsersAsync(ProjectFilterParams projectFilterParams);

        Task<Project> FindWithUserAndCollaboratorsAsync(int id);

        /// <summary>
        ///     Get the number of projects
        /// </summary>
        /// <param name="projectFilterParams">The parameters to filter, sort and paginate the projects</param>
        /// <returns>The number of projects</returns>
        Task<int> ProjectsCount(ProjectFilterParams projectFilterParams);

        /// <summary>
        ///     Get the total number of pages for the results
        /// </summary>
        /// <param name="projectFilterParams">The parameters to filter, sort and paginate the projects</param>
        /// <returns>The total number of pages for the results</returns>
        Task<int> GetProjectsTotalPages(ProjectFilterParams projectFilterParams);

    }

    public class ProjectService : Service<Project>, IProjectService
    {

        public ProjectService(IProjectRepository repository) : base(repository) { }

        protected new IProjectRepository Repository => (IProjectRepository) base.Repository;

        /// <summary>
        /// Get a list of all the projects
        /// </summary>
        /// <param name="projectFilterParams">The parameters to filter, sort and paginate the projects</param>
        /// <returns>A list of all the projects</returns>
        public Task<List<Project>> GetAllWithUsersAsync(ProjectFilterParams projectFilterParams)
        {
            if(!projectFilterParams.AmountOnPage.HasValue ||
               projectFilterParams.AmountOnPage <= 0)
                projectFilterParams.AmountOnPage = 20;

            int? skip = null;
            int? take = null;
            if(projectFilterParams.Page.HasValue)
            {
                skip = projectFilterParams.AmountOnPage * (projectFilterParams.Page - 1);
                take = projectFilterParams.AmountOnPage;
            }

            Expression<Func<Project, object>> orderBy;
            switch(projectFilterParams.SortBy)
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

            bool orderByDirection = projectFilterParams.SortDirection == "asc";
            return Repository.GetAllWithUsersAsync(skip, take, orderBy, orderByDirection, projectFilterParams.Highlighted);
        }

        /// <summary>
        ///     Get the number of projects
        /// </summary>
        /// <param name="projectFilterParams">The parameters to filter, sort and paginate the projects</param>
        /// <returns>The number of projects</returns>
        public virtual async Task<int> ProjectsCount(ProjectFilterParams projectFilterParams)
        {
            return await Repository.CountAsync(projectFilterParams.Highlighted);
        }

        /// <summary>
        ///     Get the total number of pages for the results
        /// </summary>
        /// <param name="projectFilterParams">The parameters to filter, sort and paginate the projects</param>
        /// <returns>The total number of pages for the results</returns>
        public virtual async Task<int> GetProjectsTotalPages(ProjectFilterParams projectFilterParams)
        {
            if(projectFilterParams.AmountOnPage == null ||
               projectFilterParams.AmountOnPage <= 0)
                projectFilterParams.AmountOnPage = 20;
            int count = await ProjectsCount(projectFilterParams);
            return (int) Math.Ceiling(count / (decimal) projectFilterParams.AmountOnPage);
        }

        public Task<Project> FindWithUserAndCollaboratorsAsync(int id)
        {
            return Repository.FindWithUserAndCollaboratorsAsync(id);
        }

    }

}
