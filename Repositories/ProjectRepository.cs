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


using Data;
using MessageBrokerPublisher;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Defaults;
using Models.Exceptions;
using Newtonsoft.Json.Linq;
using Repositories.Base;
using Repositories.ElasticSearch;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
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
        /// <param name="categories"></param>
        /// <param name="tags"></param>
        /// <returns>List of projects</returns>
        Task<List<Project>> GetAllWithUsersCollaboratorsAndInstitutionsAsync(
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null,
            ICollection<int> categories = null,
            ICollection<int> tags = null
        );

        /// <summary>
        /// Returns project with a NON redacted User. Do not use when you do not need the user's email adress.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Project> FindAsyncNotRedacted(int id);

        /// <summary>
        ///     This interface method counts the amount of projects matching the filters.
        /// </summary>
        /// <param name="highlighted"></param>
        /// <param name="categories">The categories parameter represents the categories the project needs to have</param>
        /// <returns>number of projects found</returns>
        Task<int> CountAsync(bool? highlighted = null, ICollection<int> categories = null, ICollection<int> tags = null, int? userId = null);

        /// <summary>
        ///     This interface method searches the database for projects matching the search query and parameters.
        /// </summary>
        /// <param name="query">The query parameters represents the search query used for filtering projects.</param>
        /// <param name="skip">The skip parameter represents the number of projects to skip.</param>
        /// <param name="take">The take parameter represents the number of projects to return.</param>
        /// <param name="orderBy">The order by parameter represents the way how to order the projects.</param>
        /// <param name="orderByAsc">The order by asc parameters represents the order direction (True: asc, False: desc)</param>
        /// <param name="highlighted">The highlighted parameter represents the whether to filter highlighted projects.</param>
        /// <param name="categories">The categories parameter represents the categories the project needs to have</param>
        /// <returns>This method returns thee projects matching the search query and parameters.</returns>
        Task<IEnumerable<Project>> SearchAsync(
            string query,
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null,
            ICollection<int> categories = null,
            ICollection<int> tags = null
        );

        /// <summary>
        ///     This interface method counts the amount of projects matching the filters and the search query.
        /// </summary>
        /// <param name="query">The query parameters represents the search query used for filtering projects.</param>
        /// <param name="highlighted">The highlighted parameter represents the whether to filter highlighted projects.</param>
        /// <param name="categories">The categories parameter represents the categories the project needs to have</param>
        /// <returns>This method returns the amount of projects matching the filters.</returns>
        Task<int> SearchCountAsync(string query, bool? highlighted = null, ICollection<int> categories = null, ICollection<int> tags = null);


        Task SyncProjectToES(Project project);


        /// <summary>
        ///     This method will retrieve a project with user and collaborators async. Project will be redacted if user
        ///     has that setting configured.
        /// </summary>
        /// <param name="id">The unique identifier which is used for searching the correct project.</param>
        /// <returns>
        ///     This method returns possibly redacted Project object with user and collaborators.
        /// </returns>
        Task<Project> FindWithUserCollaboratorsAndInstitutionsAsync(int id);

        Task<bool> ProjectExistsAsync(int id);

        /// <summary>
        ///     Get the user projects.
        /// </summary>
        /// <param name="userId">The id of the user whoms projects need to be retrieved</param>
        /// <returns>A enumerable of the users projects</returns>
        Task<IEnumerable<Project>> GetUserProjects(int userId,
                                                   int? skip = null,
                                                   int? take = null,
                                                   Expression<Func<Project, object>> orderBy = null,
                                                   bool orderByAsc = true,
                                                   bool? highlighted = null,
                                                   ICollection<int> categories = null,
                                                   ICollection<int> tags = null);
        Task<List<Project>> GetLikedProjectsFromSimilarUser(int userId, int similarUserId);
        void CreateProjectIndex();
        void DeleteIndex();
        void MigrateDatabase(List<Project> projectsToExport);

        /// <summary>
        ///     This method will call the ElasticSearch index to retrieve projects where the title starts with the query.
        /// </summary>
        /// <param name="query">The string of characters with which the title must begin</param>
        /// <returns>
        ///     This method return a list of projects where the title, or part of the title matches the query.
        /// </returns>
        Task<List<Project>> FindProjectsWhereTitleStartsWithQuery(string query);
    }

    /// <summary>
    ///     This is the project repository
    /// </summary>
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly ITaskPublisher taskPublisher;
        private readonly RestClient elasticSearchContext;
        private readonly Queries queries;

        /// <summary>
        ///     This is the constructor of the project repository
        /// </summary>
        /// <param name="dbContext"></param>
        public ProjectRepository(DbContext dbContext) : base(dbContext) { }


        public ProjectRepository(DbContext dbContext, IElasticSearchContext elasticSearchContext, ITaskPublisher taskPublisher, Queries queries) : base(dbContext)
        {
            this.taskPublisher = taskPublisher;
            this.elasticSearchContext = elasticSearchContext.CreateRestClientForElasticRequests();
            this.queries = queries;
        }

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
                                    .Include(p => p.CallToActions)
                                    .Include(p => p.Images)
                                    .Include(p => p.Tags)
                                    .SingleOrDefaultAsync();

            if(project != null)
            {
                project.Collaborators = await GetDbSet<Collaborator>()
                                              .Where(p => p.ProjectId == project.Id)
                                              .ToListAsync();
                project.Likes = await GetDbSet<ProjectLike>()
                                      .Where(p => p.LikedProject.Id == project.Id)
                                      .ToListAsync();
                project.Categories = await GetDbSet<ProjectCategory>()
                                      .Include(p => p.Category)
                                      .Where(p => p.Project.Id == project.Id)
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
        /// <param name="categories">The categories parameter represents the categories the project needs to have</param>
        /// <param name="tags">The tags parameter represents the tags the project needs to have</param>
        /// <returns>This method returns a list of projects filtered by the specified parameters.</returns>
        public virtual async Task<List<Project>> GetAllWithUsersCollaboratorsAndInstitutionsAsync(
                    int? skip = null,
                    int? take = null,
                    Expression<Func<Project, object>> orderBy = null,
                    bool orderByAsc = true,
                    bool? highlighted = null,
                    ICollection<int> categories = null,
                    ICollection<int> tags = null
                )
        {
            IQueryable<Project> queryableProjects = GetDbSet<Project>()
                .Include(u => u.User)
                .Include(p => p.ProjectIcon)
                .Include(p => p.CallToActions)
                .Include(p => p.Collaborators)
                .Include(p => p.Likes)
                .Include(p => p.LinkedInstitutions)
                .Include(p => p.Categories)
                .ThenInclude(c => c.Category)
                .Include(p => p.Tags)
                .ThenInclude(t => t.Tag);

            queryableProjects = ApplyFilters(queryableProjects, skip, take, orderBy, orderByAsc, highlighted, categories, tags);

            //Execute the IQueryable to get a collection of results
            //Don't get the description for performance reasons.
            List<Project> projectResults = await queryableProjects.Select(p => new Project
            {
                UserId = p.UserId,
                User = p.User,
                Id = p.Id,
                ProjectIconId = p.ProjectIconId,
                ProjectIcon = p.ProjectIcon,
                CallToActions = p.CallToActions,
                Collaborators = p.Collaborators,
                Likes = p.Likes,
                LinkedInstitutions = p.LinkedInstitutions,
                Categories = p.Categories.Select(c => new ProjectCategory()
                {
                    Category = c.Category,
                    Id = c.Id
                }).ToList(),
                Tags = p.Tags.Select(t => new ProjectTag()
                {
                    Tag = t.Tag,
                    Id = t.Id
                }).ToList(),
                Created = p.Created,
                InstitutePrivate = p.InstitutePrivate,
                Name = p.Name,
                ShortDescription = p.ShortDescription,
                Updated = p.Updated,
                Uri = p.Uri
            })
                .ToListAsync();
            //Redact the user after fetching the collection from the project (no separate query needs to be executed)
            projectResults.ForEach(project => project.User = RedactUser(project.User));
            return projectResults;
        }

        /// <summary>
        ///     This method counts the amount of projects matching the filters.
        /// </summary>
        /// <param name="highlighted">The highlighted parameter represents whether to filter highlighted projects.</param>
        /// <param name="categories">The categories parameter represents the categories the project needs to have</param>
        /// <returns>This method returns the amount of projects matching the filters.</returns>
        public virtual async Task<int> CountAsync(bool? highlighted = null, ICollection<int> categories = null, ICollection<int> tags = null, int? userId = null)
        {
            if(userId.HasValue)
            {
                return await ApplyFilters(DbSet, null, null, null, true, highlighted, categories, tags)
                             .Where(p => p.UserId == userId)
                             .CountAsync();
            }
            return await ApplyFilters(DbSet, null, null, null, true, highlighted, categories, tags)
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
        /// <param name="categories">The categories parameter represents the categories the project needs to have</param>
        /// <returns>This method returns thee projects matching the search query and parameters.</returns>
        public virtual async Task<IEnumerable<Project>> SearchAsync(
            string query,
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null,
            ICollection<int> categories = null,
            ICollection<int> tags = null
        )
        {
            List<Project> result =
                await ApplyFilters(await GetProjectQueryable(query), skip, take, orderBy, orderByAsc, highlighted, categories, tags)
                    .ToListAsync();
            return result.Where(p => ProjectContainsQuery(p, query))
                         .ToList();
        }

        /// <summary>
        ///     This method counts the amount of projects matching the filters and the search query.
        /// </summary>
        /// <param name="query">The query parameters represents the search query used for filtering projects.</param>
        /// <param name="highlighted">The highlighted parameter represents the whether to filter highlighted projects.</param>
        /// <param name="categories">The categories parameter represents the categories the project needs to have</param>
        /// <returns>This method returns the amount of projects matching the filters.</returns>
        public virtual async Task<int> SearchCountAsync(string query, bool? highlighted = null, ICollection<int> categories = null, ICollection<int> tags = null)
        {
            return await ApplyFilters(await GetProjectQueryable(query), null, null, null, true, highlighted, categories, tags)
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
        public async Task<Project> FindWithUserCollaboratorsAndInstitutionsAsync(int id)
        {
            Project project = await GetDbSet<Project>()
                                    .Include(p => p.User)
                                    .Include(p => p.ProjectIcon)
                                    .Include(p => p.CallToActions)
                                    .Include(p => p.Images)
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
                project.Categories = await GetDbSet<ProjectCategory>()
                                      .Include(p => p.Category)
                                      .Where(p => p.Project.Id == project.Id)
                                      .ToListAsync();
                project.Tags = await GetDbSet<ProjectTag>()
                                      .Include(p => p.Tag)
                                      .Where(p => p.Project.Id == project.Id)
                                      .ToListAsync();

                project.LinkedInstitutions = await GetDbSet<ProjectInstitution>()
                                                    .Include(p => p.Institution)
                                                    .Where(p => p.ProjectId == project.Id)
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
            SetLikes(entity);
            ESProjectDTO projectToSync = ProjectConverter.ProjectToESProjectDTO(entity);
            taskPublisher.RegisterTask(Newtonsoft.Json.JsonConvert.SerializeObject(projectToSync), Subject.ELASTIC_CREATE_OR_UPDATE);


        }

        private static void SetLikes(Project entity)
        {
            if(entity != null)
            {
                if(entity.Likes != null)
                {
                    _ = entity.Likes;
                }

            }
        }

        /// <summary>
        /// This method adds the given project to the database and ES Index.
        /// </summary>
        /// <param name="entity">The entity parameter represents a project to be added.</param>
        public override void Add(Project entity)
        {
            base.Add(entity);
            base.Save();
            Console.WriteLine("Add method id: " + entity.Id);
            SetLikes(entity);
            ESProjectDTO projectToSync = ProjectConverter.ProjectToESProjectDTO(entity);
            taskPublisher.RegisterTask(Newtonsoft.Json.JsonConvert.SerializeObject(projectToSync), Subject.ELASTIC_CREATE_OR_UPDATE);
        }

        public override void AddRange(IEnumerable<Project> entities)
        {
            List<Project> entityList = entities.ToList();
            for(int i = 0; i < entityList.Count; i++)
            {
                entityList[i] = UpdateCreatedField(entityList[i]);
                entityList[i] = UpdateUpdatedField(entityList[i]);
            }
            DbSet.AddRange(entityList);
            entityList.ForEach(x =>
            {
                SetLikes(x);
                ESProjectDTO projectToSync = ProjectConverter.ProjectToESProjectDTO(x);
                taskPublisher.RegisterTask(Newtonsoft.Json.JsonConvert.SerializeObject(projectToSync),
                    Subject.ELASTIC_CREATE_OR_UPDATE);
            });

        }

        /// <summary>
        /// This method asynchronously adds the project to the database and ES Index.
        /// </summary>
        /// <param name="entity">The project parameter represents the project to be added.</param>
        /// <returns>
        /// The task wherein the addition of the project from the database is executed.
        /// </returns>
        public override async Task AddAsync(Project entity)
        {

            await base.AddAsync(entity);
            base.Save();
            Console.WriteLine("AddAsync method id: " + entity.Id);
            SetLikes(entity);
            ESProjectDTO projectToSync = ProjectConverter.ProjectToESProjectDTO(entity);
            taskPublisher.RegisterTask(Newtonsoft.Json.JsonConvert.SerializeObject(projectToSync), Subject.ELASTIC_CREATE_OR_UPDATE);
        }

        /// <summary>
        /// This method removes the project from the database and ES Index.
        /// </summary>
        /// <param name="entity">The entity parameter represents a project to be removed.</param>
        public override void Remove(Project entity)
        {
            base.Remove(entity);
            SetLikes(entity);
            ESProjectDTO projectToSync = ProjectConverter.ProjectToESProjectDTO(entity);
            taskPublisher.RegisterTask(Newtonsoft.Json.JsonConvert.SerializeObject(projectToSync), Subject.ELASTIC_DELETE);
        }

        /// <summary>
        ///     Get the user projects.
        /// </summary>
        /// <param name="userId">The id of the user whoms projects need to be retrieved</param>
        /// <param name="skip">The skip parameter represents the number of projects to skip.</param>
        /// <param name="take">The take parameter represents the number of projects to return.</param>
        /// <param name="orderBy">The order by parameter represents the way how to order the projects.</param>
        /// <param name="orderByAsc">The order by asc parameters represents the order direction (True: asc, False: desc)</param>
        /// <param name="highlighted">The highlighted parameter represents the whether to filter highlighted projects.</param>
        /// <param name="categories">The categories parameter represents the categories the project needs to have</param>
        /// <returns>A enumerable of the users projects</returns>
        public async Task<IEnumerable<Project>> GetUserProjects(
            int userId,
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null,
            ICollection<int> categories = null, ICollection<int> tags = null)
        {
            IQueryable<Project> projects = GetDbSet<Project>()
                                           .Include(p => p.Collaborators)
                                           .Include(p => p.ProjectIcon)
                                           .Include(p => p.Images)
                                           .Include(p => p.Categories)
                                           .ThenInclude(c => c.Category)
                                           .Include(p => p.Tags)
                                           .ThenInclude(t => t.Tag)
                                           .Where(p => p.UserId == userId);

            projects = ApplyFilters(projects, skip, take, orderBy, orderByAsc, highlighted, categories, tags);

            List<Project> projectResults = await projects.Select(p => new Project
            {
                UserId = p.UserId,
                User = p.User,
                Id = p.Id,
                ProjectIconId = p.ProjectIconId,
                ProjectIcon = p.ProjectIcon,
                CallToActions = p.CallToActions,
                Collaborators = p.Collaborators,
                Likes = p.Likes,
                LinkedInstitutions = p.LinkedInstitutions,
                Categories = p.Categories.Select(c => new ProjectCategory()
                {
                    Category = c.Category,
                    Id = c.Id
                }).ToList(),
                Created = p.Created,
                InstitutePrivate = p.InstitutePrivate,
                Name = p.Name,
                ShortDescription = p.ShortDescription,
                Updated = p.Updated,
                Uri = p.Uri
            })
                    .ToListAsync();
            return projectResults;
        }

        /// <summary>
        ///     This method redacts user email from the Project if isPublic setting is set to false.
        /// This method asynchronously removes the project from the database and ES Index coupled to the given id.
        /// </summary>
        /// <param name="id">The project parameter represents unique identifier.</param>
        /// <returns>
        /// The task wherein the removal of the project from the database is executed.
        /// </returns>
        public override Task RemoveAsync(int id)
        {
            Project entity = DbSet.Find(id);
            SetLikes(entity);
            ESProjectDTO projectToSync = ProjectConverter.ProjectToESProjectDTO(entity);
            taskPublisher.RegisterTask(Newtonsoft.Json.JsonConvert.SerializeObject(projectToSync), Subject.ELASTIC_DELETE);
            return base.RemoveAsync(id);
        }


        /// <summary>
        /// This method redacts user email from the Project if isPublic setting is set to false.
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
        /// <param name="categories"></param>
        /// <param name="tags"></param>
        /// <returns>
        ///     This method returns a IQueryable Projects collection based on the given filters.
        /// </returns>
        private IQueryable<Project> ApplyFilters(
            IQueryable<Project> queryable,
            int? skip,
            int? take,
            Expression<Func<Project, object>> orderBy,
            bool orderByAsc,
            bool? highlighted,
            ICollection<int> categories,
            ICollection<int> tags
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

            if(categories != null && categories.Count > 0)
            {
                // Check for every project if there is any project-category that matches the selected categories
                // queryable = queryable.Where(p => p.Categories.All(cat => categories.Contains(cat.Category.Id)));
                queryable = queryable.Where(p => p.Categories.Any(cat => categories.Contains(cat.Category.Id)));
            }

            if(tags != null && tags.Count > 0)
            {
                queryable = queryable.Where(p => p.Tags.Any(tag => tags.Contains(tag.Id)));
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

            if(skip.HasValue && skip.Value > 0) queryable = queryable.Skip(skip.Value);
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
            Regex regex = new Regex(@$"{query}", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex wholeWordRegex = new Regex(@$"\b{query}\b", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return new List<string>
                   {
                       project.Description,
                       project.ShortDescription,
                       project.Uri,
                       project.User.Name,
                       project.Id.ToString()
                   }
                .Any(text => wholeWordRegex.IsMatch(text))
                || regex.IsMatch(project.Name);
        }

        /// <summary>
        ///     This method gets the project queryable which contains the provided query.
        /// </summary>
        /// <param name="query">The query parameter is a string to search in the project's fields.</param>
        /// <returns>This method returns the filtered IQueryable including the project user.</returns>
        private async Task<IQueryable<Project>> GetProjectQueryable(string query)
        {
            IQueryable<Project> projectsToReturn = GetDbSet<Project>()
                                                   .Include(u => u.User)
                                                   .Where(p =>
                                                              p.Name.Contains(query) ||
                                                              p.Description.Contains(query) ||
                                                              p.ShortDescription.Contains(query) ||
                                                              p.Uri.Contains(query) ||
                                                              p.Id.ToString()
                                                               .Equals(query) ||
                                                              p.User.Name.Contains(query));
            projectsToReturn.Include(p => p.ProjectIcon).Load();
            projectsToReturn.Include(p => p.CallToActions).Load();
            projectsToReturn.Include(p => p.Likes).Load();
            projectsToReturn.Include(p => p.Categories).Load();
            projectsToReturn.Include(p => p.Tags).Load();

            foreach(Project project in projectsToReturn)
            {
                project.Collaborators = await GetDbSet<Collaborator>()
                                              .Where(p => p.ProjectId == project.Id)
                                              .ToListAsync();
                project.Likes = await GetDbSet<ProjectLike>()
                                      .Where(p => p.LikedProject.Id == project.Id)
                                      .ToListAsync();
                project.Categories = await GetDbSet<ProjectCategory>()
                                      .Include(p => p.Category)
                                      .Where(p => p.Project.Id == project.Id)
                                      .ToListAsync();
                project.Tags = await GetDbSet<ProjectTag>()
                                      .Include(p => p.Tag)
                                      .Where(p => p.Project.Id == project.Id)
                                      .ToListAsync();
            }
            return projectsToReturn;
        }

        public void CreateProjectIndex()
        {
            string body = queries.IndexProjects;
            RestRequest request = new RestRequest(Method.PUT);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            elasticSearchContext.Execute(request);
        }


        public async Task<List<Project>> GetLikedProjectsFromSimilarUser(int userId, int similarUserId)
        {
            RestRequest request = new RestRequest("_search", Method.POST);
            string body = queries.ProjectRecommendations
                .Replace("ReplaceWithUserId", userId.ToString())
                .Replace("ReplaceWithSimilarUserId", similarUserId.ToString());
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse restResponse = elasticSearchContext.Execute(request);

            List<ESProjectDTO> esProjectFormats = new List<ESProjectDTO>();

            ParseJsonToESProjectFormatDTOList(restResponse, esProjectFormats);
            return await ConvertProjects(esProjectFormats);

        }

        private void ParseJsonToESProjectFormatDTOList(IRestResponse restResponse, List<ESProjectDTO> esProjectFormats)
        {
            JObject esProjects = JObject.Parse(restResponse.Content);
            JToken projects = esProjects.GetValue("hits")["hits"];
            foreach(JToken project in projects)
            {
                esProjectFormats.Add(project.Last.First.ToObject<ESProjectDTO>());
            }
        }

        private async Task<List<Project>> ConvertProjects(List<ESProjectDTO> elasticSearchProjects)
        {
            List<Project> projects = new List<Project>();
            foreach(ESProjectDTO p in elasticSearchProjects)
            {
                projects.Add(await FindWithUserCollaboratorsAndInstitutionsAsync(p.Id));
            }
            return projects;
        }


        public void DeleteIndex()
        {
            RestRequest request = new RestRequest(Method.DELETE);
            elasticSearchContext.Execute(request);
        }

        public void MigrateDatabase(List<Project> projectsToExport)
        {
            // Converts the Projects to Elastic format.
            List<ESProjectDTO> projectsToExportDTOs = ProjectConverter.ProjectsToProjectESDTOs(projectsToExport);
            foreach(ESProjectDTO project in projectsToExportDTOs)
            {
                // Registers an Elastic formatted project at the message broker to be inserted into Elastic DB.
                taskPublisher.RegisterTask(Newtonsoft.Json.JsonConvert.SerializeObject(project), Subject.ELASTIC_CREATE_OR_UPDATE);
            }
        }

        public async Task SyncProjectToES(Project project)
        {
            Project projectToSync = await FindAsync(project.Id);
            if(projectToSync == null)
            {
                throw new NotFoundException("Project to sync was not found");
            }
            ESProjectDTO eSProjectDTO = ProjectConverter.ProjectToESProjectDTO(projectToSync);
            taskPublisher.RegisterTask(Newtonsoft.Json.JsonConvert.SerializeObject(eSProjectDTO), Subject.ELASTIC_CREATE_OR_UPDATE);


        }

        /// <summary>
        ///     This method will call the ElasticSearch index to retrieve projects where the title starts with the query.
        /// </summary>
        /// <param name="query">The string of characters with which the title must begin</param>
        /// <returns>
        ///     This method return a list of projects where the title, or part of the title matches the query.
        /// </returns>
        public async Task<List<Project>> FindProjectsWhereTitleStartsWithQuery(string query)
        {
            RestRequest request = new RestRequest("_search", Method.POST);
            string body = queries.SearchProjectsByPartOfTitle
                .Replace("ReplaceWithQuery", query);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse restResponse = elasticSearchContext.Execute(request);
            if(!restResponse.IsSuccessful && restResponse.Content == "")
            {
                throw new ElasticUnavailableException();
            }
            List<ESProjectDTO> esProjectFormats = new List<ESProjectDTO>();

            ParseJsonToESProjectFormatDTOList(restResponse, esProjectFormats);
            return await ConvertProjects(esProjectFormats);
        }
        public Task<bool> ProjectExistsAsync(int id)
        {
            return GetDbSet<Project>().AnyAsync(i => i.Id == id);
        }

        public Task<Project> FindAsyncNotRedacted(int id)
        {
            return GetDbSet<Project>().Where(p => p.Id == id).Include(u => u.User).FirstOrDefaultAsync();
        }
    }
}
