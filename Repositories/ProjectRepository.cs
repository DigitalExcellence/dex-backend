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

    /// <summary>
    ///     This is the interface of the project repository
    /// </summary>
    public interface IProjectRepository : IRepository<Project>
    {

        /// <summary>
        ///     This interface method gets all projects including their owner and collaborators
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderByAsc"></param>
        /// <param name="highlighted"></param>
        /// <returns>List of projects</returns>
        Task<List<Project>> GetAllWithUsersAndCollaboratorsAsync(
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null
        );

        /// <summary>
        ///     This interface method counts the amount of projects matching the filters.
        /// </summary>
        /// <param name="highlighted"></param>
        /// <returns>number of projects found</returns>
        Task<int> CountAsync(bool? highlighted = null);

        /// <summary>
        ///     This interface method searches the database for projects matching the search query and parameters.
        /// </summary>
        /// <param name="query">The query parameters represents the search query used for filtering projects.</param>
        /// <param name="skip">The skip parameter represents the number of projects to skip.</param>
        /// <param name="take">The take parameter represents the number of projects to return.</param>
        /// <param name="orderBy">The order by parameter represents the way how to order the projects.</param>
        /// <param name="orderByAsc">The order by asc parameters represents the order direction (True: asc, False: desc)</param>
        /// <param name="highlighted">The highlighted parameter represents the whether to filter highlighted projects.</param>
        /// <returns>This method returns thee projects matching the search query and parameters.</returns>
        Task<IEnumerable<Project>> SearchAsync(
            string query,
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null
        );

        /// <summary>
        ///     This interface method counts the amount of projects matching the filters and the search query.
        /// </summary>
        /// <param name="query">The query parameters represents the search query used for filtering projects.</param>
        /// <param name="highlighted">The highlighted parameter represents the whether to filter highlighted projects.</param>
        /// <returns>This method returns the amount of projects matching the filters.</returns>
        Task<int> SearchCountAsync(string query, bool? highlighted = null);

        /// <summary>
        ///     This method will retrieve a project with user and collaborators async. Project will be redacted if user
        ///     has that setting configured.
        /// </summary>
        /// <param name="id">The unique identifier which is used for searching the correct project.</param>
        /// <returns>
        ///     This method returns possibly redacted Project object with user and collaborators.
        /// </returns>
        Task<Project> FindWithUserAndCollaboratorsAsync(int id);

        /// <summary>
        ///     Get the user projects.
        /// </summary>
        /// <param name="userId">The id of the user whoms projects need to be retrieved</param>
        /// <returns>A enumerable of the users projects</returns>
        Task<IEnumerable<Project>> GetUserProjects(int userId);
    }

    /// <summary>
    ///     This is the project repository
    /// </summary>
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {

        /// <summary>
        ///     This is the constructor of the project repository
        /// </summary>
        /// <param name="dbContext"></param>
        public ProjectRepository(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        ///     This method finds the project async by project the specified id.
        /// </summary>
        /// <param name="id">The unique identifier which is used for searching the correct project.</param>
        /// <returns>
        ///     This method returns a project with the specified id with possibly redacted email.
        /// </returns>
        public override async Task<Project> FindAsync(int id)
        {
            Project project = await GetDbSet<Project>()
                                    .Where(s => s.Id == id)
                                    .Include(p => p.ProjectIcon)
                                    .Include(p => p.CallToAction)
                                    .SingleOrDefaultAsync();

            if(project != null)
            {
                project.Collaborators = await GetDbSet<Collaborator>()
                                              .Where(p => p.ProjectId == project.Id)
                                              .ToListAsync();
                project.Likes = await GetDbSet<ProjectLike>()
                                      .Where(p => p.LikedProject.Id == project.Id)
                                      .ToListAsync();
            }

            return RedactUser(project);
        }

        /// <summary>
        ///     This method gets all the projects in the database.
        /// </summary>
        /// <param name="skip">The skip parameter represents the number of projects to skip.</param>
        /// <param name="take">The take parameter represents the number of projects to return.</param>
        /// <param name="orderBy">The order by parameter represents the way how to order the projects.</param>
        /// <param name="orderByAsc">The order by asc parameters represents the order direction (True: asc, False: desc)</param>
        /// <param name="highlighted">The highlighted parameter represents the whether to filter highlighted projects.</param>
        /// <returns>This method returns a list of projects filtered by the specified parameters.</returns>
        public virtual async Task<List<Project>> GetAllWithUsersAndCollaboratorsAsync(
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null
        )
        {
            IQueryable<Project> queryableProjects = GetDbSet<Project>()
                                                    .Include(u => u.User)
                                                    .Include(p => p.ProjectIcon)
                                                    .Include(p => p.CallToAction)
                                                    .Include( p => p.Collaborators )
                                                    .Include( p => p.User )
                                                    .Include( p => p.Likes );

            queryableProjects = ApplyFilters(queryableProjects, skip, take, orderBy, orderByAsc, highlighted);

            //Execute the IQueryable to get a collection of results
            List<Project> projectResults = await queryableProjects.ToListAsync();

            //Redact the user after fetching the collection from the project (no separate query needs to be executed)
            projectResults.ForEach( project => project.User = RedactUser( project.User ) );

            return projectResults;
        }

        /// <summary>
        ///     This method counts the amount of projects matching the filters.
        /// </summary>
        /// <param name="highlighted">The highlighted parameter represents whether to filter highlighted projects.</param>
        /// <returns>This method returns the amount of projects matching the filters.</returns>
        public virtual async Task<int> CountAsync(bool? highlighted = null)
        {
            return await ApplyFilters(DbSet, null, null, null, true, highlighted)
                       .CountAsync();
        }

        /// <summary>
        ///     This method searches the database for projects matching the search query and parameters.
        /// </summary>
        /// <param name="query">The query parameters represents the search query used for filtering projects.</param>
        /// <param name="skip">The skip parameter represents the number of projects to skip.</param>
        /// <param name="take">The take parameter represents the number of projects to return.</param>
        /// <param name="orderBy">The order by parameter represents the way how to order the projects.</param>
        /// <param name="orderByAsc">The order by asc parameters represents the order direction (True: asc, False: desc)</param>
        /// <param name="highlighted">The highlighted parameter represents the whether to filter highlighted projects.</param>
        /// <returns>This method returns thee projects matching the search query and parameters.</returns>
        public virtual async Task<IEnumerable<Project>> SearchAsync(
            string query,
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null
        )
        {
            List<Project> result =
                await ApplyFilters(await GetProjectQueryable(query), skip, take, orderBy, orderByAsc, highlighted)
                    .ToListAsync();
            return result.Where(p => ProjectContainsQuery(p, query))
                         .ToList();
        }

        /// <summary>
        ///     This method counts the amount of projects matching the filters and the search query.
        /// </summary>
        /// <param name="query">The query parameters represents the search query used for filtering projects.</param>
        /// <param name="highlighted">The highlighted parameter represents the whether to filter highlighted projects.</param>
        /// <returns>This method returns the amount of projects matching the filters.</returns>
        public virtual async Task<int> SearchCountAsync(string query, bool? highlighted = null)
        {
            return await ApplyFilters(await GetProjectQueryable(query), null, null, null, true, highlighted)
                       .CountAsync();
        }

        /// <summary>
        ///     This method will retrieve a project with user and collaborators async. Project will be redacted if user
        ///     has that setting configured.
        /// </summary>
        /// <param name="id">The unique identifier which is used for searching the correct project.</param>
        /// <returns>
        ///     This method returns possibly redacted Project object with user and collaborators.
        /// </returns>
        public async Task<Project> FindWithUserAndCollaboratorsAsync(int id)
        {
            Project project = await GetDbSet<Project>()
                                    .Include(p => p.User)
                                    .Include(p => p.ProjectIcon)
                                    .Include(p => p.CallToAction)
                                    .Where(p => p.Id == id)
                                    .FirstOrDefaultAsync();
            if(project != null)
            {
                project.Collaborators = await GetDbSet<Collaborator>()
                                              .Where(p => p.ProjectId == project.Id)
                                              .ToListAsync();
                project.Likes = await GetDbSet<ProjectLike>()
                                      .Where(p => p.LikedProject.Id == project.Id)
                                      .ToListAsync();
            }

            return RedactUser(project);
        }

        /// <summary>
        ///     This method updates the specified entity excluding the user object.
        /// </summary>
        /// <param name="entity">The entity parameter represents the updated project object.</param>
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
        ///     Get the user projects.
        /// </summary>
        /// <param name="userId">The id of the user whoms projects need to be retrieved</param>
        /// <returns>A enumerable of the users projects</returns>
        public async Task<IEnumerable<Project>> GetUserProjects(int userId)
        {
            IEnumerable<Project> projects = await GetDbSet<Project>()
                   .Include(p => p.Collaborators)
                   .Include(p => p.ProjectIcon)
                   .Where(p => p.UserId == userId)
                   .ToListAsync();

            return projects;
        }

        /// <summary>
        ///     This method redacts user email from the Project if isPublic setting is set to false.
        /// </summary>
        /// <param name="project">The project parameter represents the project object that will be used.</param>
        /// <returns>
        ///     This method returns the project with possibly redacted email depending on setting.
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
        ///     This method redacts the user email from the User if isPublic setting is set to false.
        /// </summary>
        /// <param name="user">The user parameter represents the user object that will be used.</param>
        /// <returns>
        ///     This method returns the user with possibly redacted email depending on setting.
        /// </returns>
        private User RedactUser(User user)
        {
            if(user == null) return null;
            if(user.IsPublic == false)
            {
                user.Email = Defaults.Privacy.RedactedEmail;
            }
            return user;
        }

        /// <summary>
        ///     This method redacts the user email from the Projects in the list. The email will only be
        ///     redacted if isPublic setting is set to false.
        /// </summary>
        /// <param name="projects">The projects parameter represents the project objects that will be used.</param>
        /// <returns>
        ///     This method returns a list of Projects with possibly redacted email depending on setting.
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
        ///     This method applies query parameters and find project based on these filters.
        /// </summary>
        /// <param name="queryable">The linq queryable parameter represents the IQueryable object.</param>
        /// <param name="skip">The skip parameter represents the number of projects to skip.</param>
        /// <param name="take">The take parameter represents the number of projects to return.</param>
        /// <param name="orderBy">The order by parameter represents the way how to order the projects.</param>
        /// <param name="orderByAsc">The order by asc parameters represents the order direction (True: asc, False: desc)</param>
        /// <param name="highlighted">The highlighted parameter represents the whether to filter highlighted projects.</param>
        /// <returns>
        ///     This method returns a IQueryable Projects collection based on the given filters.
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
        ///     This method checks if any of the searchable fields of the project passed contains the provided query.
        /// </summary>
        /// <param name="project">The project parameter represents a Project to search in.</param>
        /// <param name="query">The query parameter represents the query to search in the project's searchable fields.</param>
        /// <returns>
        ///     This method returns a boolean representing whether or not the passed query was found in the
        ///     searchable fields of the provided project.
        /// </returns>
        private static bool ProjectContainsQuery(Project project, string query)
        {
            Regex regex = new Regex(@$"\b{query}\b", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return new List<string>
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
        ///     This method gets the project queryable which contains the provided query.
        /// </summary>
        /// <param name="query">The query parameter is a string to search in the project's fields.</param>
        /// <returns>This method returns the filtered IQueryable including the project user.</returns>
        private async Task<IQueryable<Project>> GetProjectQueryable(string query)
        {
            IQueryable<Project> projectsToReturn = DbSet
                                                   .Include(p => p.User)
                                                   .Include(i => i.ProjectIcon)
                                                   .Include(p => p.CallToAction)
                                                   .Include(l => l.Likes)
                                                   .Where(p =>
                                                              p.Name.Contains(query) ||
                                                              p.Description.Contains(query) ||
                                                              p.ShortDescription.Contains(query) ||
                                                              p.Uri.Contains(query) ||
                                                              p.Id.ToString()
                                                               .Equals(query) ||
                                                              p.User.Name.Contains(query));

            foreach(Project project in projectsToReturn)
            {
                project.Collaborators = await GetDbSet<Collaborator>()
                                              .Where(p => p.ProjectId == project.Id)
                                              .ToListAsync();
                project.Likes = await GetDbSet<ProjectLike>()
                                      .Where(p => p.LikedProject.Id == project.Id)
                                      .ToListAsync();
            }
            return projectsToReturn;
        }

    }

}
