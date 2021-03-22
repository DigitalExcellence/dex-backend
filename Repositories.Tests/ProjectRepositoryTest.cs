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
using FluentAssertions;
using MessageBrokerPublisher;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Defaults;
using Models.Exceptions;
using Moq;
using NUnit.Framework;
using Repositories.ElasticSearch;
using Repositories.Tests.Base;
using Repositories.Tests.DataSources;
using Repositories.Tests.Extensions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Repositories.Tests
{

    [TestFixture]
    public class ProjectRepositoryTest : RepositoryTest<Project, ProjectRepository>
    {
        protected new ApplicationDbContext DbContext;
        protected new IProjectRepository Repository;
        protected Mock<IElasticSearchContext> ElasticSearchContext;
        protected Mock<ITaskPublisher> TaskPublisher;
        protected Mock<Queries> Queries;
        protected Mock<RestClient> RestClientMock;


        /// <summary>
        /// Initialize runs before every test
        /// Initialize the repository with reflection
        /// </summary>
        [SetUp]
        public override void Initialize()
        {
            DbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);            
            TaskPublisher = new Mock<ITaskPublisher>();
            Queries = new Mock<Queries>();
            ElasticSearchContext = new Mock<IElasticSearchContext>();
            RestClientMock = new Mock<RestClient>();
            ElasticSearchContext.Setup(x => x.CreateRestClientForElasticRequests()).Returns(RestClientMock.Object);

            Repository = new ProjectRepository(DbContext, ElasticSearchContext.Object, TaskPublisher.Object, Queries.Object);
        }


        [Test]
        public async Task SyncProjectToESTest_Goodflow([ProjectDataSource(1)] Project project)
        {
            DbContext.Add(project);
            await DbContext.SaveChangesAsync();
            ESProjectDTO dto = new ESProjectDTO();
            ProjectToEsProjectESDTO(project, dto);
            string payload = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
            Subject subject = Subject.ELASTIC_CREATE_OR_UPDATE;
            TaskPublisher.Setup(x => x.RegisterTask(It.Is<string>(x => x == payload), It.Is<Subject>(x => x == subject))).Verifiable();
            

            await Repository.SyncProjectToES(project);

            TaskPublisher.VerifyAll();


        }

        [Test]
        public void SyncProjectToESTest_Badflow([ProjectDataSource(1)] Project project)
        {
            ESProjectDTO dto = new ESProjectDTO();
            ProjectToEsProjectESDTO(project, dto);
            string payload = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
            Subject subject = Subject.ELASTIC_CREATE_OR_UPDATE;


            TaskPublisher.Setup(x => x.RegisterTask(It.Is<string>(x => x == payload), It.Is<Subject>(x => x == subject))).Verifiable();


            Assert.ThrowsAsync<NotFoundException>(async () => await Repository.SyncProjectToES(project));

        }

        [Test]
        public void MigratDatabaseTest_Goodflow([ProjectDataSource(30)] List<Project> projects)
        {
            List<ESProjectDTO> dTOs = ProjectsToProjectESDTO(projects);
            Subject subject = Subject.ELASTIC_CREATE_OR_UPDATE;

            TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<string>(), It.Is<Subject>(x => x == subject))).Verifiable();

            Repository.MigrateDatabase(projects);

            TaskPublisher.Verify(x => x.RegisterTask(It.IsAny<string>(), It.Is<Subject>(x => x == subject)), Times.Exactly(projects.Count));

            TaskPublisher.VerifyAll();

        }

        [Test]
        public void DeleteIndexTest_Goodflow()
        {
            Repository.DeleteIndex();

            RestClientMock.Verify(x => x.Execute(It.Is<RestRequest>( x => x.Method == Method.DELETE)), Times.Once);
        }

        [Test]
        public void CreateProjectIndex_Goodflow()
        {
            Repository.CreateProjectIndex();
            RestClientMock.Verify(x => x.Execute(It.Is<RestRequest>(x => x.Method == Method.PUT)), Times.Once);
        }

        [Test]
        public void GetLikedProjectsFromSimilarUser_Goodflow()
        {

            
        }








        /// <summary>
        /// Test if project with user relations are retrieved correctly
        /// </summary>
        /// <param name="projects">The project which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllWithUserAsyncTest_GoodFlow(
            [ProjectDataSource(100)] List<Project> projects,
            [UserDataSource(100)] List<User> users)
        {
            // Set project - user relation and seed database
            for(int i = 0; i < projects.Count; i++)
            {
                projects[i]
                    .User = users[i];
            }

            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            // Test
            List<Project> retrieved = await Repository.GetAllWithUsersAndCollaboratorsAsync();
            Assert.AreEqual(100, retrieved.Count);
            foreach(Project project in projects)
            {
                Assert.IsNotNull(project.User);
            }
        }

        /// <summary>
        ///     Test if no project are retrieved when the database is empty
        /// </summary>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllWithUsersAsyncTest_NoProjects([UserDataSource(100)] List<User> users)
        {
            await DbContext.AddRangeAsync(users);
            await DbContext.SaveChangesAsync();

            // Test
            List<Project> retrieved = await Repository.GetAllWithUsersAndCollaboratorsAsync();
            Assert.AreEqual(0, retrieved.Count);
        }

        /// <summary>
        ///     Test if no project are retrieved when the required relation with users is missing
        /// </summary>
        /// <param name="projects">The project which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllWithUsersAsyncTest_NoUsers(
            [ProjectDataSource(100)] List<Project> projects)
        {
            // Seed database
            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            // Test
            List<Project> retrieved = await Repository.GetAllWithUsersAndCollaboratorsAsync();
            Assert.AreEqual(0, retrieved.Count);
        }

        /// <summary>
        ///     Check if the get all projects is able to skip the correct amount of projects
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllSkipTakeAsyncTest_GoodFlow(
            [ProjectDataSource(100)] List<Project> projects,
            [UserDataSource(100)] List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            List<Project> retrieved = await Repository.GetAllWithUsersAndCollaboratorsAsync(0, 1);
            Assert.AreEqual(1, retrieved.Count, "Get all with skip take failed");

            retrieved = await Repository.GetAllWithUsersAndCollaboratorsAsync(0, 10);
            Assert.AreEqual(10, retrieved.Count, "Get all with skip take failed");

            retrieved = await Repository.GetAllWithUsersAndCollaboratorsAsync(10, 10);
            Assert.AreEqual(10, retrieved.Count, "Get all with skip take failed");
        }

        /// <summary>
        ///     Check if the get all projects is returning no projects when skipping all the projects in the result
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllSkipTakeAsyncTest_BadFlow_SkipAllProjects(
            [ProjectDataSource(100)] List<Project> projects,
            [UserDataSource(100)] List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            List<Project> retrieved = await Repository.GetAllWithUsersAndCollaboratorsAsync(1000, 10);
            Assert.AreEqual(0, retrieved.Count);
        }

        /// <summary>
        ///     Check if the count is counting the correct amount of projects
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task CountAsyncTest_GoodFlow(
            [ProjectDataSource(100)] List<Project> projects,
            [UserDataSource(100)] List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            int retrieved = await Repository.CountAsync();
            Assert.AreEqual(100, retrieved);
        }

        /// <summary>
        ///     Check if the count is returning 0 when there are no projects in the database
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CountAsyncTest_BadFlow_NoProjects()
        {
            // Tests
            int retrieved = await Repository.CountAsync();
            Assert.AreEqual(0, retrieved);
        }

        /// <summary>
        ///     Test getallwithusersasync function to see if it adheres the ispublic flag on the project user.
        /// </summary>
        /// <param name="project">The project that will be seeded.</param>
        /// <param name="user">The user that will be seeded.</param>
        [Test]
        public async Task GetAllWithUsersAsyncTest_IsPublicEmail_False(
            [ProjectDataSource] Project project,
            [UserDataSource] User user)
        {
            user.IsPublic = false;
            DbContext.Add(user);
            project.User = user;

            // Seed database
            DbContext.Add(project);
            await DbContext.SaveChangesAsync();

            // Test
            List<Project> retrieved = await Repository.GetAllWithUsersAndCollaboratorsAsync();
            Assert.AreEqual(1, retrieved.Count);
            Assert.AreEqual(retrieved[0]
                            .User.Email,
                            Defaults.Privacy.RedactedEmail);
        }

        /// <summary>
        ///     Test getallwithusersasync function to see if it adheres the ispublic flag on the project user.
        /// </summary>
        /// <param name="project">The project that will be seeded.</param>
        /// <param name="user">The user that will be seeded.</param>
        [Test]
        public async Task GetAllWithUsersAsyncTest_IsPublicEmail_True(
            [ProjectDataSource] Project project,
            [UserDataSource] User user)
        {
            user.IsPublic = true;
            DbContext.Add(user);
            project.User = user;

            // Seed database
            DbContext.Add(project);
            await DbContext.SaveChangesAsync();

            // Test
            List<Project> retrieved = await Repository.GetAllWithUsersAndCollaboratorsAsync();
            Assert.AreEqual(1, retrieved.Count);
            Assert.AreEqual(retrieved[0]
                            .User.Email,
                            user.Email);
        }


        /// <summary>
        ///     Check if the search is returning the correct project based on the search terms
        /// </summary>
        /// <param name="projects">The project which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchAsyncTest_GoodFlow(
            [ProjectDataSource(100)] List<Project> projects,
            [UserDataSource(100)] List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            // Id search
            List<Project> retrieved = (List<Project>) await Repository.SearchAsync("1");
            foreach(Project project in retrieved)
            {
                Assert.True(project.Id.ToString()
                                   .Contains("1"),
                            "Id search failed");
            }

            // Name search
            retrieved = (List<Project>) await Repository.SearchAsync("exName");
            Assert.AreEqual(50, retrieved.Count, "Name search failed");

            // Description search
            retrieved = (List<Project>) await Repository.SearchAsync("defex");
            Assert.AreEqual(50, retrieved.Count, "Description search failed");

            // Short Description search
            retrieved = (List<Project>) await Repository.SearchAsync("ghijex");
            Assert.AreEqual(50, retrieved.Count, "ShortDescription search failed");

            // Uri search
            retrieved = (List<Project>) await Repository.SearchAsync("example");
            Assert.AreEqual(50, retrieved.Count, "Uri search failed");

            // User name search
            retrieved = (List<Project>) await Repository.SearchAsync("xyzex");
            Assert.AreEqual(50, retrieved.Count, "User name search failed");

            // Combined search
            retrieved = (List<Project>) await Repository.SearchAsync("ex");
            Assert.AreEqual(0, retrieved.Count, "Combined search failed");
        }

        /// <summary>
        ///     Check if the search is returning no project when the database is empty
        /// </summary>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchAsyncTest_BadFlow_NoProjects(
            [UserDataSource(100)] List<User> users)
        {
            // Seed database
            await DbContext.AddRangeAsync(users);
            await DbContext.SaveChangesAsync();

            // Test
            List<Project> retrieved = (List<Project>) await Repository.SearchAsync("abc");
            Assert.AreEqual(0, retrieved.Count);
        }

        /// <summary>
        ///     Check if the search is returning no project when the required relation with users is missing
        /// </summary>
        /// <param name="projects">The project which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchAsyncTest_BadFlow_NoUsers([ProjectDataSource(100)] List<Project> projects)
        {
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects);

            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            List<Project> retrieved = (List<Project>) await Repository.SearchAsync("abc");
            Assert.AreEqual(0, retrieved.Count);
        }

        /// <summary>
        ///     Check if the search is returning no project if non of the project has the included string in any of the properties
        /// </summary>
        /// <param name="projects">The project which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchAsyncTest_BadFlow_NoMatching(
            [ProjectDataSource(100)] List<Project> projects,
            [UserDataSource(100)] List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            // Id search
            List<Project> retrieved = (List<Project>) await Repository.SearchAsync("-1");
            Assert.AreEqual(0, retrieved.Count);

            // Name search
            retrieved = (List<Project>) await Repository.SearchAsync("test");
            Assert.AreEqual(0, retrieved.Count);
        }

        /// <summary>
        ///     Check if the search is able to skip the correct amount of project while returning the correct search results
        /// </summary>
        /// <param name="projects">The project which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchSkipTakeAsyncTest_GoodFlow(
            [ProjectDataSource(100)] List<Project> projects,
            [UserDataSource(100)] List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            // Id search
            List<Project> retrieved = (List<Project>) await Repository.SearchAsync("1", 0, 1);
            Assert.AreEqual(1, retrieved.Count, "Id search failed");

            // Name search
            retrieved = (List<Project>) await Repository.SearchAsync("exName", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "Name search failed");

            // Description search
            retrieved = (List<Project>) await Repository.SearchAsync("defex", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "Description search failed");

            // Short Description search
            retrieved = (List<Project>) await Repository.SearchAsync("ghijex", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "Short Description search failed");

            // Uri search
            retrieved = (List<Project>) await Repository.SearchAsync("example", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "Uri search failed");

            // User name search
            retrieved = (List<Project>) await Repository.SearchAsync("xyzex", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "User name search failed");

            // Combined search
            retrieved = (List<Project>) await Repository.SearchAsync("ex", 10, 40);
            Assert.AreEqual(0, retrieved.Count, "Combined search failed");
        }

        /// <summary>
        ///     Check if the search is returning no project when skipping all the project in the result
        /// </summary>
        /// <param name="projects">The project which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchSkipTakeAsyncTest_BadFlow_SkipAllProjects(
            [ProjectDataSource(100)] List<Project> projects,
            [UserDataSource(100)] List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            List<Project> retrieved = (List<Project>) await Repository.SearchAsync("ex", 1000, 10);
            Assert.AreEqual(0, retrieved.Count);
        }

        /// <summary>
        ///     Check if the count is counting the correct amount of project
        /// </summary>
        /// <param name="projects">The project which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchCountAsyncTest_GoodFlow(
            [ProjectDataSource(100)] List<Project> projects,
            [UserDataSource(100)] List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            int retrieved = await Repository.SearchCountAsync("ex");
            Assert.AreEqual(100, retrieved);
        }

        /// <summary>
        ///     Check if the count is returning 0 when there are no matching project
        /// </summary>
        /// <param name="projects">The project which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchCountAsyncTest_BadFlow_NoMatchingProjects(
            [ProjectDataSource(100)] List<Project> projects,
            [UserDataSource(100)] List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            int retrieved = await Repository.SearchCountAsync("randomSearchWhichDoesntMatch");
            Assert.AreEqual(0, retrieved);
        }

        /// <summary>
        ///     Check if the count is returning 0 when there are no project in the database
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SearchCountAsyncTest_BadFlow_NoProjects()
        {
            // Tests
            int retrieved = await Repository.SearchCountAsync("1");
            Assert.AreEqual(0, retrieved);
        }

        /// <summary>
        ///     Checks if the correct project with user and collaborators are found
        /// </summary>
        /// <param name="projects">The project which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <param name="collaborators">The collaborator that will be seededs which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task FindWithUserAndCollaboratorsAsyncTest_GoodFlow(
            [ProjectDataSource(100)] List<Project> projects,
            [UserDataSource(100)] List<User> users,
            [CollaboratorDataSource(400)] List<Collaborator> collaborators)
        {
            // Seeding
            projects = SetStaticTestData(projects, users);
            int i = 0;
            foreach(Project project in projects)
            {
                project.Collaborators = new List<Collaborator>();
                for(int a = i; a < collaborators.Count; a++)
                {
                    project.Collaborators.Add(collaborators[a]);
                    i = a;
                }
            }

            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            // Testing
            Project retrieved = await Repository.FindWithUserAndCollaboratorsAsync(1);
            Assert.AreEqual(projects[0]
                                .Id,
                            retrieved.Id);
            Assert.AreEqual(projects[0]
                                .Name,
                            retrieved.Name);
            Assert.AreEqual(projects[0]
                                .ShortDescription,
                            retrieved.ShortDescription);
            Assert.AreEqual(projects[0]
                                .Description,
                            retrieved.Description);
            Assert.AreEqual(projects[0]
                                .Uri,
                            retrieved.Uri);
            Assert.AreEqual(projects[0]
                            .User.Name,
                            retrieved.User.Name);
            Assert.AreEqual(projects[0]
                            .Collaborators[0]
                            .FullName,
                            retrieved.Collaborators[0]
                                     .FullName);
        }

        /// <summary>
        ///     Finds the with user and collaborators asynchronous test is public true.
        /// </summary>
        /// <param name="project">The project that will be seeded.</param>
        /// <param name="user">The user that will be seeded.</param>
        /// <param name="collaborator">The collaborator that will be seeded.</param>
        [Test]
        public async Task FindWithUserAndCollaboratorsAsyncTest_IsPublic_True(
            [ProjectDataSource] Project project,
            [UserDataSource] User user,
            [CollaboratorDataSource] Collaborator collaborator)
        {
            user.IsPublic = true;
            project.User = user;
            project.Collaborators.Add(collaborator);

            // Seeding
            DbContext.Add(user);
            DbContext.Add(collaborator);
            DbContext.Add(project);

            await DbContext.SaveChangesAsync();

            // Testing
            Project retrieved = await Repository.FindWithUserAndCollaboratorsAsync(project.Id);
            Assert.AreEqual(project.Id, retrieved.Id);
            Assert.AreEqual(project.Name, retrieved.Name);
            Assert.AreEqual(project.ShortDescription, retrieved.ShortDescription);
            Assert.AreEqual(project.Description, retrieved.Description);
            Assert.AreEqual(project.Uri, retrieved.Uri);
            Assert.AreEqual(project.User.Name, retrieved.User.Name);
            Assert.AreEqual(project.User.Email, retrieved.User.Email);
            Assert.AreEqual(project.Collaborators[0]
                                   .FullName,
                            retrieved.Collaborators[0]
                                     .FullName);
        }

        /// <summary>
        ///     Finds the with user and collaborators asynchronous test is public true.
        /// </summary>
        /// <param name="project">The project that will be seeded.</param>
        /// <param name="user">The user that will be seeded.</param>
        /// <param name="collaborator">The collaborator that will be seeded.</param>
        [Test]
        public async Task FindWithUserAndCollaboratorsAsyncTest_IsPublic_False(
            [ProjectDataSource] Project project,
            [UserDataSource] User user,
            [CollaboratorDataSource] Collaborator collaborator)
        {
            user.IsPublic = false;
            project.User = user;
            project.Collaborators.Add(collaborator);

            // Seeding
            DbContext.Add(user);
            DbContext.Add(collaborator);
            DbContext.Add(project);

            await DbContext.SaveChangesAsync();

            // Testing
            Project retrieved = await Repository.FindWithUserAndCollaboratorsAsync(project.Id);
            Assert.AreEqual(project.Id, retrieved.Id);
            Assert.AreEqual(project.Name, retrieved.Name);
            Assert.AreEqual(project.ShortDescription, retrieved.ShortDescription);
            Assert.AreEqual(project.Description, retrieved.Description);
            Assert.AreEqual(project.Uri, retrieved.Uri);
            Assert.AreEqual(project.User.Name, retrieved.User.Name);
            Assert.AreEqual(project.User.Email, Defaults.Privacy.RedactedEmail);
            Assert.AreEqual(project.Collaborators[0]
                                   .FullName,
                            retrieved.Collaborators[0]
                                     .FullName);
        }

        /// <summary>
        ///     Checks if the result is null when the database has no project
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task FindWithUserAndCollaboratorsAsyncTest_BadFlow_NoProjects()
        {
            // Testing
            Project retrieved = await Repository.FindWithUserAndCollaboratorsAsync(1);
            Assert.IsNull(retrieved);
        }

        /// <summary>
        ///     Checks if the result is null when the required relation with user is missing
        /// </summary>
        /// <param name="projects">The project which are used as data to test</param>
        /// <param name="collaborators">The collaborators which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task FindWithUserAndCollaboratorsAsyncTest_BadFlow_NoUsers(
            [ProjectDataSource(100)] List<Project> projects,
            [CollaboratorDataSource(400)] List<Collaborator> collaborators)
        {
            // Seeding
            projects = SetStaticTestData(projects);
            int i = 0;
            foreach(Project project in projects)
            {
                project.Collaborators = new List<Collaborator>();
                for(int a = i; a < collaborators.Count; a++)
                {
                    project.Collaborators.Add(collaborators[a]);
                    i = a;
                }
            }

            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            // Testing
            Project retrieved = await Repository.FindWithUserAndCollaboratorsAsync(1);

            // Retrieved is null because there are no users and users is required
            Assert.IsNull(retrieved);
        }

        /// <summary>
        ///     Checks if the amount of collaborators is 0 when the database has no collaborators
        /// </summary>
        /// <param name="projects">The project which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task FindWithUserAndCollaboratorsAsyncTest_BadFlow_NoCollaborators(
            [ProjectDataSource(100)] List<Project> projects,
            [UserDataSource(100)] List<User> users)
        {
            // Seeding
            projects = SetStaticTestData(projects, users);

            await DbContext.AddRangeAsync(projects);
            await DbContext.SaveChangesAsync();

            // Testing
            Project retrieved = await Repository.FindWithUserAndCollaboratorsAsync(1);
            Assert.AreEqual(projects[0]
                                .Id,
                            retrieved.Id);
            Assert.AreEqual(projects[0]
                                .Name,
                            retrieved.Name);
            Assert.AreEqual(projects[0]
                                .ShortDescription,
                            retrieved.ShortDescription);
            Assert.AreEqual(projects[0]
                                .Description,
                            retrieved.Description);
            Assert.AreEqual(projects[0]
                                .Uri,
                            retrieved.Uri);
            Assert.AreEqual(projects[0]
                            .User.Name,
                            retrieved.User.Name);
            Assert.AreEqual(0, retrieved.Collaborators.Count);
        }


        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override async Task AddAsyncTest_GoodFlow([ProjectDataSource]Project entity)
        {
            TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<string>(),
                It.Is<Subject>(x => x == Subject.ELASTIC_CREATE_OR_UPDATE))).Verifiable();

            Repository.Add(entity);
            await DbContext.SaveChangesAsync();
            
            PropertyInfo property = entity.GetType().GetProperty("Id");
            int id;
            if(property == null)
            {
                throw new Exception("Id property does not exist");
            } else
            {
                id = (int) property.GetValue(entity);
            }

            Repository.Invoking(async r => await r.FindAsync(id)).Should().NotBeNull();
            TaskPublisher.Verify();
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override void AddRangeTest_BadFlow_EmptyList()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<String>(),
                It.Is<Subject>(x => x == Subject.ELASTIC_CREATE_OR_UPDATE))).Verifiable();
                Repository.Add(null);
                await DbContext.SaveChangesAsync();
            });
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override void AddRangeTest_BadFlow_Null()
        {
            TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<String>(),
            It.Is<Subject>(x => x == Subject.ELASTIC_CREATE_OR_UPDATE))).Verifiable();
            Assert.Throws<ArgumentNullException>(() => Repository.AddRange(null));
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override async Task AddRangeTest_GoodFlow([ProjectDataSource(5)]List<Project> entities)
        {
            TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<String>(),
                It.Is<Subject>(x => x == Subject.ELASTIC_CREATE_OR_UPDATE))).Verifiable();
            Repository.AddRange(entities);
            await DbContext.SaveChangesAsync();

            foreach(Project entity in entities)
            {
                int id = entity.Id;                
                Repository.Invoking(async r => await r.FindAsync(id)).Should().NotBeNull();
                TaskPublisher.Verify();
            }
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override void AddTest_BadFlow_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<String>(),
                It.Is<Subject>(x => x == Subject.ELASTIC_CREATE_OR_UPDATE))).Verifiable();
                Repository.Add(null);
                await DbContext.SaveChangesAsync();
            });
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public async Task FindAsyncTest_BadFlow_NotExists(
            [ProjectDataSource] Project project,
            [CollaboratorDataSource(10)] List<Collaborator> collaborators)
        {
            project.Collaborators = collaborators;
            DbContext.Add(project);
            await DbContext.SaveChangesAsync();

            Assert.IsNull(await Repository.FindAsync(-1));
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public async Task FindAsyncTest_GoodFlow([ProjectDataSource] Project project,
                                                 [CollaboratorDataSource(10)] List<Collaborator> collaborators)
        {
            project.Collaborators = collaborators;
            DbContext.Add(project);
            await DbContext.SaveChangesAsync();

            Project retrieved = await Repository.FindAsync(1);
            Assert.AreEqual(project.Id, retrieved.Id);
            Assert.AreEqual(project.Name, retrieved.Name);
            Assert.AreEqual(project.Description, retrieved.Description);
            Assert.AreEqual(project.ShortDescription, retrieved.ShortDescription);
            Assert.AreEqual(project.Uri, retrieved.Uri);
            Assert.AreEqual(project.Collaborators.Count, retrieved.Collaborators.Count);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override async Task GetAllAsyncTest_Badflow_Empty()
        {
            TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<String>(),
                It.Is<Subject>(x => x == Subject.ELASTIC_CREATE_OR_UPDATE))).Verifiable();

            List<Project> retrievedEntities = (List<Project>) await Repository.GetAll();
            Assert.AreEqual(0, retrievedEntities.Count);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override async Task GetAllAsyncTest_GoodFlow([ProjectDataSource(5)]List<Project> entities)
        {
            int amountToTest = entities.Count;
            TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<String>(),
                It.Is<Subject>(x => x == Subject.ELASTIC_CREATE_OR_UPDATE))).Verifiable();

            await DbContext.AddRangeAsync(entities);
            await DbContext.SaveChangesAsync();

            List<Project> retrievedEntities = (List<Project>) await Repository.GetAll();
            Assert.AreEqual(amountToTest, retrievedEntities.Count);
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override async Task RemoveAsyncTest_BadFlow_NotExists([ProjectDataSource]Project entity)
        {
            TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<String>(),
                It.Is<Subject>(x => x == Subject.ELASTIC_CREATE_OR_UPDATE))).Verifiable();
            Repository.Add(entity);
            TaskPublisher.Verify();
            await DbContext.SaveChangesAsync();            
            Assert.ThrowsAsync<NullReferenceException>(async () => await Repository.RemoveAsync(-1));

        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override async Task RemoveAsyncTest_GoodFlow([ProjectDataSource]Project entity)
        {
            TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<String>(),
                It.Is<Subject>(x => x == Subject.ELASTIC_CREATE_OR_UPDATE))).Verifiable();
            Repository.Add(entity);
            TaskPublisher.Verify();
            await DbContext.SaveChangesAsync();

            Type type = entity.GetType();
            PropertyInfo property = type.GetProperty("Id");
            int id;
            if(property == null)
            {
                throw new Exception("Id property does not exist");
            } else
            {
                id = (int) property.GetValue(entity);
            }

            TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<String>(),
                It.Is<Subject>(x => x == Subject.ELASTIC_DELETE))).Verifiable();

            await Repository.RemoveAsync(id);
            await DbContext.SaveChangesAsync();
            TaskPublisher.Verify();
            Assert.NotNull(Repository.FindAsync(id));
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override async Task UpdateTest_BadFlow_NotExists([ProjectDataSource]Project entity, [ProjectDataSource]Project updateEntity)
        {
            TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<String>(),
                It.Is<Subject>(x => x == Subject.ELASTIC_CREATE_OR_UPDATE))).Verifiable();

            Repository.Add(entity);
            await DbContext.SaveChangesAsync();

            Type type = entity.GetType();
            PropertyInfo property = type.GetProperty("Id");
            if(property == null)
            {
                throw new Exception("Id property does not exist");
            } else
            {
                property.SetValue(updateEntity, -1);
            }

            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                Repository.Update(updateEntity);
                await DbContext.SaveChangesAsync();
            });
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override async Task UpdateTest_BadFlow_Null([ProjectDataSource]Project entity)
        {
            TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<String>(),
                It.Is<Subject>(x => x == Subject.ELASTIC_CREATE_OR_UPDATE))).Verifiable();

            Repository.Add(entity);
            await DbContext.SaveChangesAsync();

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                Repository.Update(null);
                await DbContext.SaveChangesAsync();
            });
        }

        /// <inheritdoc cref="RepositoryTest{TDomain, TRepository}" />
        [Test]
        public override async Task UpdateTest_GoodFlow([ProjectDataSource]Project entity, [ProjectDataSource]Project updateEntity)
        {
            Project copy = entity.CloneObject<Project>();
            TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<String>(),
                It.Is<Subject>(x => x == Subject.ELASTIC_CREATE_OR_UPDATE))).Verifiable();

            Repository.Add(entity);
            await DbContext.SaveChangesAsync();

            Type type = entity.GetType();
            PropertyInfo property = type.GetProperty("Id");
            int id;
            if(property == null)
            {
                throw new Exception("Id property does not exist");
            } else
            {
                id = (int) property.GetValue(entity);
            }
            TaskPublisher.Setup(x => x.RegisterTask(It.IsAny<String>(),
                It.Is<Subject>(x => x == Subject.ELASTIC_CREATE_OR_UPDATE))).Verifiable();

            Repository.Update(entity);
            await DbContext.SaveChangesAsync();
            Assert.AreEqual(entity, await Repository.FindAsync(id));
            Assert.AreNotEqual(copy, await Repository.FindAsync(id));
            TaskPublisher.VerifyAll();
        }

        private List<Project> SetStaticTestData(List<Project> projects, List<User> users = null)
        {
            for(int i = 0; i < projects.Count; i++)
            {
                projects[i]
                    .Name = "exName";
                projects[i]
                    .Description = "";
                projects[i]
                    .ShortDescription = "";
                projects[i]
                    .Uri = "";
                if(users != null)
                {
                    projects[i]
                        .User = users[i];
                    projects[i]
                        .User.Name = "";
                }
            }

            for(int i = 0; i < 50; i++)
            {
                projects[i]
                    .Name = "abcex";
                projects[i]
                    .Description = "defex";
                projects[i]
                    .ShortDescription = "ghijex";
                projects[i]
                    .Uri = "https://www.example.com/";
                if(users != null)
                {
                    projects[i]
                        .User.Name = "xyzex";
                }
            }

            return projects;
        }

        private void ProjectToEsProjectESDTO(Project project, ESProjectDTO dto)
        {
            List<int> likes = new List<int>();
            if(project.Likes != null)
            {
                foreach(ProjectLike projectLike in project.Likes)
                {
                    likes.Add(projectLike.UserId);
                }
            }
            dto.Description = project.Description;
            dto.ProjectName = project.Name;
            dto.Id = project.Id;
            dto.Created = project.Created;
            dto.Likes = likes;
        }

        private List<ESProjectDTO> ProjectsToProjectESDTO(List<Project> projects)
        {
            List<ESProjectDTO> convertedProjects = new List<ESProjectDTO>();
            foreach(Project project in projects)
            {
                ESProjectDTO convertedProject = new ESProjectDTO();
                ProjectToEsProjectESDTO(project, convertedProject);
                convertedProjects.Add(convertedProject);

            }
            return convertedProjects;
        }

    }

}
