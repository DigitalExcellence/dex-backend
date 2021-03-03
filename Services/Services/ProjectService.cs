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

using Ganss.XSS;
using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Services
{

    /// <summary>
    ///     This is the interface of the project service
    /// </summary>
    public interface IProjectService : IService<Project>
    {

        /// <summary>
        ///     Get a list of all the projects
        /// </summary>
        /// <param name="projectFilterParams">The parameters to filter, sort and paginate the projects</param>
        /// <returns>A list of all the projects</returns>
        Task<List<Project>> GetAllWithUsersAndCollaboratorsAsync(ProjectFilterParams projectFilterParams);

        /// <summary>
        ///     Gets a project including owner and collaborators
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Project entity</returns>
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

        /// <summary>
        ///     Get the users projects
        /// </summary>
        /// <param name="userId">The user id whoms projects need to be retrieved</param>
        /// <returns>The total number of pages for the results</returns>
        Task<IEnumerable<Project>> GetUserProjects(int userId);
    }

    /// <summary>
    ///     This is the project service
    /// </summary>
    public class ProjectService : Service<Project>, IProjectService
    {

        /// <summary>
        ///     This is the project service constructor
        /// </summary>
        /// <param name="repository"></param>
        public ProjectService(IProjectRepository repository) : base(repository) { }

        /// <summary>
        ///     Gets the repository
        /// </summary>
        protected new IProjectRepository Repository => (IProjectRepository) base.Repository;

        /// <summary>
        ///     This is a overridden add method
        /// </summary>
        /// <param name="entity"></param>
        public override void Add(Project entity)
        {
            // Sanitize description before executing default behaviour.
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            entity.Description = sanitizer.Sanitize(entity.Description);
            base.Add(entity);
        }

        /// <summary>
        ///     This is a overridden update method
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(Project entity)
        {
            // Sanitize description before executing default behaviour.
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            entity.Description = sanitizer.Sanitize(entity.Description);
            base.Update(entity);
        }

        /// <summary>
        ///     Get a list of all the projects
        /// </summary>
        /// <param name="projectFilterParams">The parameters to filter, sort and paginate the projects</param>
        /// <returns>A list of all the projects</returns>
        public Task<List<Project>> GetAllWithUsersAndCollaboratorsAsync(ProjectFilterParams projectFilterParams)
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
            return Repository.GetAllWithUsersAndCollaboratorsAsync(skip,
                                                                   take,
                                                                   orderBy,
                                                                   orderByDirection,
                                                                   projectFilterParams.Highlighted);
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

        /// <summary>
        ///     Gets a project with owner and collaborators asynchronous
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A project entity</returns>
        public Task<Project> FindWithUserAndCollaboratorsAsync(int id)
        {
            return Repository.FindWithUserAndCollaboratorsAsync(id);
        }

        public Task<IEnumerable<Project>> GetUserProjects(int userId)
        {
            return Repository.GetUserProjects(userId);
        }
    }

}
