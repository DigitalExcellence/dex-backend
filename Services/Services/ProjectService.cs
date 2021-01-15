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

using AngleSharp;
using Ganss.XSS;
using MessageBrokerPublisher;
using Models;
using Repositories;
using RestSharp;
using RestSharp.Authenticators;
using Services.Base;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;
using System.Linq;
using Services.Resources;

namespace Services.Services
{

    public interface IProjectService : IService<Project>
    {

        /// <summary>
        ///     Get a list of all the projects
        /// </summary>
        /// <param name="projectFilterParams">The parameters to filter, sort and paginate the projects</param>
        /// <returns>A list of all the projects</returns>
        Task<List<Project>> GetAllWithUsersAndCollaboratorsAsync(ProjectFilterParams projectFilterParams);

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
        ///     Returns all projects and projects in the format of ElasticSearch.
        /// </summary>
        /// <returns></returns>
        Task<List<ESProjectFormat>> GetAllESProjectsFromProjects();

        /// <summary>
        ///     Registers all records of the current database to the message broker to be added to ElasticSearch.
        /// </summary>
        /// <param name="projectsToExport"></param>
        void MigrateDatabase(List<ESProjectFormat> projectsToExport);

    }

    public class ProjectService : Service<Project>, IProjectService
    {

        private INotificationSender notificationSender;
        private IConnectionFactoryElasticSearch connection;
        private ElasticConfig elasticConfig;

        public ProjectService(IProjectRepository repository, INotificationSender notificationSender, IConnectionFactoryElasticSearch connectionFactoryElasticSearch, ElasticConfig elasticConfig) : base(repository)
        {
            this.notificationSender = notificationSender;
            connection = connectionFactoryElasticSearch;
            this.elasticConfig = elasticConfig;
        }

        protected new IProjectRepository Repository => (IProjectRepository) base.Repository;

        public override void Add(Project entity)
        {
            // Sanitize description before executing default behaviour.
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            entity.Description = sanitizer.Sanitize(entity.Description);
            base.Add(entity);
        }

        public override void Update(Project entity)
        {
            // Sanitize description before executing default behaviour.
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            entity.Description = sanitizer.Sanitize(entity.Description);
            base.Update(entity);
        }

        /// <summary>
        /// Get a list of all the projects
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
            return Repository.GetAllWithUsersAndCollaboratorsAsync(skip, take, orderBy, orderByDirection, projectFilterParams.Highlighted);
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

        public async Task<List<ESProjectFormat>> GetAllESProjectsFromProjects()
        {
            IEnumerable<Project> projectsToConvert = await Repository.GetAllWithUsersAndCollaboratorsAsync();
            List<ESProjectFormat> convertedProjects = new List<ESProjectFormat>();
            foreach(Project project in projectsToConvert)
            {
                ESProjectFormat convertedProject = new ESProjectFormat();
                List<int> likes = new List<int>();
                foreach(ProjectLike projectLike in project.Likes)
                {
                    likes.Add(projectLike.UserId);
                }
                convertedProject.Description = project.Description;
                convertedProject.ProjectName = project.Name;
                convertedProject.Id = project.Id;
                convertedProject.Created = project.Created;
                convertedProject.Likes = likes;
                convertedProjects.Add(convertedProject);

            }
            return convertedProjects;
        }

        public void MigrateDatabase(List<ESProjectFormat> projectsToExport)
        {
            CreateProjectIndexElastic();
            DeleteAllPreviousDocumentsElastic();
            foreach(ESProjectFormat pr in projectsToExport)
            {
                notificationSender.RegisterNotification(Newtonsoft.Json.JsonConvert.SerializeObject(pr), Subject.ELASTIC_CREATE_OR_UPDATE);
            }
        }

        private void DeleteAllPreviousDocumentsElastic()
        {
            notificationSender.RegisterNotification(Newtonsoft.Json.JsonConvert.SerializeObject(DateTime.Now), Subject.ELASTIC_DELETE_ALL);
        }

        private void CreateProjectIndexElastic()
        {
            string body = System.IO.File.ReadAllText(Path.GetFullPath("./Resources/ElasticSearch/IndexProjects.json")).Replace("\n", "").Replace("\r", "").Replace(" ","");
            RestClient client = connection.CreateRestClientForElasticRequests();

            RestRequest request = new RestRequest(elasticConfig.IndexUrl, Method.PUT);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content + response.StatusCode);
        }

    }

}
