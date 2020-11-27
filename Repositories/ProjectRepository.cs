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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Models;
using Models.Defaults;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {

        Task<List<Project>> GetAllWithUsersAsync(
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null
            );

        Task<int> CountAsync(bool? highlighted = null);

        Task<IEnumerable<Project>> SearchAsync(
            string query,
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null
        );

        Task<int> SearchCountAsync(string query, bool? highlighted = null);

        Task<Project> FindWithUserAndCollaboratorsAsync(int id);
    }

    public class ProjectRepository : Repository<Project>, IProjectRepository
    {

        public ProjectRepository(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Redact user email from the Project if isPublic setting is set to false
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>
        /// Project with possibly redacted email depending on setting
        /// </returns>
        private Project RedactUser(Project project)
        {
            if(project == null) return null;

            if(project?.User?.IsPublic == false)
            {
                project.User.Email = Defaults.Privacy.RedactedEmail;
            }
            return project;
        }

        /// <summary>
        /// Redact user email from the Projects in the list.
        /// Email will only be redacted if isPublic setting is set to false.
        /// </summary>
        /// <param name="projects">The projects.</param>
        /// <returns>
        /// List of Projects with possibly redacted email depending on setting
        /// </returns>
        private List<Project> RedactUser(List<Project> projects)
        {
            for(int i = 0; i < projects.Count; i++)
            {
                projects[i] = RedactUser(projects[i]);
            }
            return projects;
        }

        /// <summary>
        /// Find the project async by project id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Project with possibly redacted email
        /// </returns>
        public override async Task<Project> FindAsync(int id)
        {
            Project project = await GetDbSet<Project>()
                   .Where(s => s.Id == id)
                   .Include(p => p.Collaborators)
                   .Include(p => p.CallToAction)
                   .SingleOrDefaultAsync();

            return RedactUser(project);
        }

        /// <summary>
        /// Apply query parameters and find project based on these filters
        /// </summary>
        /// <param name="queryable">The linq queryable object.</param>
        /// <param name="skip">The amount of objects to skip.</param>
        /// <param name="take">The amount of objects to take.</param>
        /// <param name="orderBy">The order by expression.</param>
        /// <param name="orderByAsc">if set to <c>true</c> [order by asc].</param>
        /// <param name="highlighted">Boolean if the project should show highlighted.</param>
        /// <returns>
        /// IQueryable Projects based on the given filters
        /// </returns>
        private IQueryable<Project> ApplyFilters(
            IQueryable<Project> queryable,
            int? skip,
            int? take,
            Expression<Func<Project, object>> orderBy,
            bool orderByAsc,
            bool? highlighted
            )
        {
            if(highlighted.HasValue)
            {
                IEnumerable<int> highlightedQueryable = DbContext.Set<Highlight>()
                                                                 .Where(h => h.StartDate <= DateTime.Now ||
                                                                             h.StartDate == null)
                                                                 .Where(h => h.EndDate >= DateTime.Now ||
                                                                             h.EndDate == null)
                                                                 .Select(h => h.ProjectId)
                                                                 .ToList();

                queryable = queryable.Where(p => highlightedQueryable.Contains(p.Id) == highlighted.Value);
            }

            if(orderBy != null)
            {
                if(orderByAsc)
                {
                    queryable = queryable.OrderBy(orderBy);
                } else
                {
                    queryable = queryable.OrderByDescending(orderBy);
                }
            }

            if(skip.HasValue) queryable = queryable.Skip(skip.Value);
            if(take.HasValue) queryable = queryable.Take(take.Value);
            return queryable;
        }

        /// <summary>
        ///     Get the projects in the database
        /// </summary>
        /// <param name="skip">The number of projects to skip</param>
        /// <param name="take">The number of projects to return</param>
        /// <param name="orderBy">The property to order the projects by</param>
        /// <param name="orderByAsc">The order direction (True: asc, False: desc)</param>
        /// <param name="highlighted">Filter highlighted projects</param>
        /// <returns>The projects filtered by the parameters</returns>
        public virtual async Task<List<Project>> GetAllWithUsersAsync(
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null
            )
        {
            IQueryable<Project> queryable = DbSet
                                            .Include(p => p.User)
                                            .Include(p => p.ProjectIcon)
                                            .Include(p => p.CallToAction);
            queryable = ApplyFilters(queryable, skip, take, orderBy, orderByAsc, highlighted);

            List<Project> projects = await queryable.ToListAsync();
            return RedactUser(projects);
        }

        /// <summary>
        /// Count the amount of projects matching the filters
        /// </summary>
        /// <param name="highlighted">The highlighted filter</param>
        /// <returns>The amount of projects matching the filters</returns>
        public virtual async Task<int> CountAsync(bool? highlighted = null)
        {
            return await ApplyFilters(DbSet, null, null, null, true, highlighted).CountAsync();
        }

        /// <summary>
        ///     Search the database for projects matching the search query and parameters
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="skip">The number of projects to skip</param>
        /// <param name="take">The number of projects to return</param>
        /// <param name="orderBy">The property to order the projects by</param>
        /// <param name="orderByAsc">The order direction (True: asc, False: desc)</param>
        /// <param name="highlighted">Filter highlighted projects</param>
        /// <returns>The projects matching the search query and parameters</returns>
        public virtual async Task<IEnumerable<Project>> SearchAsync(
            string query,
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null
        )
        {
            List<Project> result = await ApplyFilters(GetProjectQueryable(query), skip, take, orderBy, orderByAsc, highlighted).ToListAsync();
            return result.Where(p => ProjectContainsQuery(p, query)).ToList();
        }

        /// <summary>
        /// Count the amount of projects matching the filters and the search query
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="highlighted">The highlighted filter</param>
        /// <returns>The amount of projects matching the filters</returns>
        public virtual async Task<int> SearchCountAsync(string query, bool? highlighted = null)
        {
            return await ApplyFilters(GetProjectQueryable(query), null, null, null, true, highlighted).CountAsync();
        }

        /// <summary>
        /// Retrieve project with user and collaborators async.
        /// Project will be redacted if user has that setting configured.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Possibly redacted Project object with user and collaborators
        /// </returns>
        public async Task<Project> FindWithUserAndCollaboratorsAsync(int id)
        {
            Project project = await GetDbSet<Project>()
                   .Include(p => p.User)
                   .Include(p => p.Collaborators)
                   .Include(p => p.ProjectIcon)
                   .Include(p => p.CallToAction)
                   .Where(p => p.Id == id)
                   .FirstOrDefaultAsync();

            return RedactUser(project);
        }

        /// <summary>
        /// Updates the specified entity excluding the user object.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Update(Project entity)
        {
            entity = UpdateUpdatedField(entity);

            DbSet.Attach(entity);
            if(entity.User != null)
            {
                DbContext.Entry(entity.User)
                         .Property(x => x.Email)
                         .IsModified = false;

                DbContext.Entry(entity.User)
                         .State = EntityState.Unchanged;
            }

            if(entity.ProjectIcon == null)
            {
                DbContext.Entry(entity)
                         .Entity.ProjectIconId = null;
            }

            DbSet.Update(entity);
        }

        /// <summary>
        /// Checks if any of the searchable fields of the project passed contains the provided query.
        /// </summary>
        /// <param name="project">A Project to search in</param>
        /// <param name="query">The query to search in the project's searchable fields.</param>
        /// <returns>A boolean representing whether or not the passed query was found in the searchable fields of the provided project.</returns>
        private static bool ProjectContainsQuery(Project project, string query)
        {
            Regex regex = new Regex(@$"\b{query}\b", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return new List<string>()
            {
                project.Name,
                project.Description,
                project.ShortDescription,
                project.Uri,
                project.User.Name,
                project.Id.ToString()
            }
            .Any(text => regex.IsMatch(text));
        }

        /// <summary>
        /// Get the project queryable which contains the provided query.
        /// </summary>
        /// <param name="query">A string to search in the project's fields.</param>
        /// <returns>The filtered IQueryable including the project user.</returns>
        private IQueryable<Project> GetProjectQueryable(string query)
        {
            return DbSet
                    .Include(p => p.User)
                    .Include(i => i.ProjectIcon)
                    .Include(p => p.CallToAction)
                    .Where(p =>
                            p.Name.Contains(query) ||
                            p.Description.Contains(query) ||
                            p.ShortDescription.Contains(query) ||
                            p.Uri.Contains(query) ||
                            p.Id.ToString().Equals(query) ||
                            p.User.Name.Contains(query));
        }
    }
}
