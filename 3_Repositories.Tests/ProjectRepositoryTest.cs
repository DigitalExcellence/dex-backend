using Models;
using NUnit.Framework;
using Repositories.Tests.Base;
using Repositories.Tests.DataSources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Tests
{
    [TestFixture]
    public class ProjectRepositoryTest : RepositoryTest<Project, ProjectRepository>
    {
        protected new IProjectRepository Repository => (IProjectRepository)base.Repository;

        /// <summary>
        /// Test if projects with user relations are retrieved correctly
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllWithUserAsyncTest_GoodFlow(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Set project - user relation and seed database
            for (int i = 0; i < projects.Count; i++)
            {
                projects[i].User = users[i];
            }
            DbContext.AddRange(projects);
            await DbContext.SaveChangesAsync();

            // Test
            List<Project> retrieved = await Repository.GetAllWithUsersAsync();
            Assert.AreEqual(100, retrieved.Count);
            foreach(Project project in projects)
            {
                Assert.IsNotNull(project.User);
            }
        }

        /// <summary>
        /// Test if no projects are retrieved when the database is empty
        /// </summary>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllWithUsersAsyncTest_NoProjects([UserDataSource(100)]List<User> users)
        {
            DbContext.AddRange(users);
            await DbContext.SaveChangesAsync();

            // Test
            List<Project> retrieved = await Repository.GetAllWithUsersAsync();
            Assert.AreEqual(0, retrieved.Count);
        }

        /// <summary>
        /// Test if no projects are retrieved when the required relation with users is missing
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllWithUsersAsyncTest_NoUsers(
            [ProjectDataSource(100)]List<Project> projects)
        {
            // Seed database
            DbContext.AddRange(projects);
            await DbContext.SaveChangesAsync();

            // Test
            List<Project> retrieved = await Repository.GetAllWithUsersAsync();
            Assert.AreEqual(0, retrieved.Count);
        }

        /// <summary>
        /// Check if the search is returning the correct projects based on the search terms
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchAsyncTest_GoodFlow(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            DbContext.AddRange(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            // Id search
            List<Project> retrieved = (List<Project>)await Repository.SearchAsync("1");
            foreach (Project project in retrieved)
            {
                Assert.True(project.Id.ToString().Contains("1"), "Id search failed");
            }

            // Name search
            retrieved = (List<Project>)await Repository.SearchAsync("abc");
            Assert.AreEqual(50, retrieved.Count, "Name search failed");

            // Description search
            retrieved = (List<Project>)await Repository.SearchAsync("def");
            Assert.AreEqual(50, retrieved.Count, "Description search failed");

            // Short Description search
            retrieved = (List<Project>)await Repository.SearchAsync("ghij");
            Assert.AreEqual(50, retrieved.Count, "ShortDescription search failed");

            // Uri search
            retrieved = (List<Project>)await Repository.SearchAsync("example");
            Assert.AreEqual(50, retrieved.Count, "Uri search failed");

            // User name search
            retrieved = (List<Project>)await Repository.SearchAsync("xyz");
            Assert.AreEqual(50, retrieved.Count, "User name search failed");

            // Combined search
            retrieved = (List<Project>)await Repository.SearchAsync("ex");
            Assert.AreEqual(100, retrieved.Count, "Combined search failed");
        }

        /// <summary>
        /// Check if the search is returning no projects when the database is empty
        /// </summary>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchAsyncTest_BadFlow_NoProjects(
            [UserDataSource(100)]List<User> users)
        {
            // Seed database
            DbContext.AddRange(users);
            await DbContext.SaveChangesAsync();

            // Test
            List<Project> retrieved = (List<Project>)await Repository.SearchAsync("abc");
            Assert.AreEqual(0, retrieved.Count);
        }

        /// <summary>
        /// Check if the search is returning no projects when the required relation with users is missing
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchAsyncTest_BadFlow_NoUsers([ProjectDataSource(100)]List<Project> projects)
        {
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects);

            DbContext.AddRange(projects);
            await DbContext.SaveChangesAsync();

            List<Project> retrieved = (List<Project>) await Repository.SearchAsync("abc");
            Assert.AreEqual(0, retrieved.Count);
        }

        /// <summary>
        /// Check if the search is returning no projects if non of the projects has the included string in any of the properties
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchAsyncTest_BadFlow_NoMatching(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            DbContext.AddRange(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            // Id search
            List<Project> retrieved = (List<Project>)await Repository.SearchAsync("-1");
            Assert.AreEqual(0, retrieved.Count);
        }

        /// <summary>
        /// Check if the search is able to skip the correct amount of projects while returning the correct search results
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchSkipTakeAsyncTest_GoodFlow(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            DbContext.AddRange(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            // Id search
            List<Project> retrieved = (List<Project>)await Repository.SearchAsync("1", 0, 1);
            Assert.AreEqual(1, retrieved.Count, "Id search failed");

            // Name search
            retrieved = (List<Project>)await Repository.SearchAsync("abc", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "Name search failed");

            // Description search
            retrieved = (List<Project>)await Repository.SearchAsync("def", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "Description search failed");

            // Short Description search
            retrieved = (List<Project>)await Repository.SearchAsync("ghij", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "Short Description search failed");

            // Uri search
            retrieved = (List<Project>)await Repository.SearchAsync("example", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "Uri search failed");

            // User name search
            retrieved = (List<Project>)await Repository.SearchAsync("xyz", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "User name search failed");

            // Combined search
            retrieved = (List<Project>)await Repository.SearchAsync("ex", 10, 40);
            Assert.AreEqual(40, retrieved.Count, "Combined search failed");
        }

        /// <summary>
        /// Check if the search is returning no projects when skipping all the projects in the result
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchSkipTakeAsyncTest_BadFlow_SkipAllProjects(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            DbContext.AddRange(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            List<Project> retrieved = (List<Project>)await Repository.SearchAsync("ex", 1000, 10);
            Assert.AreEqual(0, retrieved.Count);
        }

        /// <summary>
        /// Check if the count is counting the correct amount of projects
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchCountAsyncTest_GoodFlow(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            DbContext.AddRange(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            int retrieved = await Repository.SearchCountAsync("ex");
            Assert.AreEqual(100, retrieved);
        }

        /// <summary>
        /// Check if the count is returning 0 when there are no matching projects
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task SearchCountAsyncTest_BadFlow_NoMatchingProjects(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            DbContext.AddRange(projects);
            await DbContext.SaveChangesAsync();

            // Tests
            int retrieved = await Repository.SearchCountAsync("randomSearchWhichDoesntMatch");
            Assert.AreEqual(0, retrieved);
        }

        /// <summary>
        /// Check if the count is returning 0 when there are no projects in the database
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
        /// Checks if the correct project with user and collaborators are found
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <param name="collaborators">The collaborators which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task FindWithUserAndCollaboratorsAsyncTest_GoodFlow(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users,
            [CollaboratorDataSource(400)]List<Collaborator> collaborators)
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

            DbContext.AddRange(projects);
            await DbContext.SaveChangesAsync();

            // Testing
            Project retrieved = await Repository.FindWithUserAndCollaboratorsAsync(1);
            Assert.AreEqual(projects[0].Id, retrieved.Id);
            Assert.AreEqual(projects[0].Name, retrieved.Name);
            Assert.AreEqual(projects[0].ShortDescription, retrieved.ShortDescription);
            Assert.AreEqual(projects[0].Description, retrieved.Description);
            Assert.AreEqual(projects[0].Uri, retrieved.Uri);
            Assert.AreEqual(projects[0].User.Name, retrieved.User.Name);
            Assert.AreEqual(projects[0].Collaborators[0].FullName, retrieved.Collaborators[0].FullName);
        }

        /// <summary>
        /// Checks if the result is null when the database has no projects
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
        /// Checks if the result is null when the required relation with user is missing
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <param name="collaborators">The collaborators which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task FindWithUserAndCollaboratorsAsyncTest_BadFlow_NoUsers(
            [ProjectDataSource(100)]List<Project> projects,
            [CollaboratorDataSource(400)]List<Collaborator> collaborators)
        {
            // Seeding
            projects = SetStaticTestData(projects);
            int i = 0;
            foreach (Project project in projects)
            {
                project.Collaborators = new List<Collaborator>();
                for (int a = i; a < collaborators.Count; a++)
                {
                    project.Collaborators.Add(collaborators[a]);
                    i = a;
                }
            }

            DbContext.AddRange(projects);
            await DbContext.SaveChangesAsync();

            // Testing
            Project retrieved = await Repository.FindWithUserAndCollaboratorsAsync(1);
            
            // Retrieved is null because there are no users and users is required
            Assert.IsNull(retrieved);
        }

        /// <summary>
        /// Checks if the amount of collaborators is 0 when the database has no collaborators
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <param name="users">The users which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task FindWithUserAndCollaboratorsAsyncTest_BadFlow_NoCollaborators(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Seeding
            projects = SetStaticTestData(projects, users);
            
            DbContext.AddRange(projects);
            await DbContext.SaveChangesAsync();

            // Testing
            Project retrieved = await Repository.FindWithUserAndCollaboratorsAsync(1);
            Assert.AreEqual(projects[0].Id, retrieved.Id);
            Assert.AreEqual(projects[0].Name, retrieved.Name);
            Assert.AreEqual(projects[0].ShortDescription, retrieved.ShortDescription);
            Assert.AreEqual(projects[0].Description, retrieved.Description);
            Assert.AreEqual(projects[0].Uri, retrieved.Uri);
            Assert.AreEqual(projects[0].User.Name, retrieved.User.Name);
            Assert.AreEqual(0, retrieved.Collaborators.Count);
        }


        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task AddAsyncTest_GoodFlow([ProjectDataSource]Project entity)
        {
            return base.AddAsyncTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override void AddRangeTest_BadFlow_EmptyList()
        {
            base.AddRangeTest_BadFlow_EmptyList();
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override void AddRangeTest_BadFlow_Null()
        {
            base.AddRangeTest_BadFlow_Null();
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task AddRangeTest_GoodFlow([ProjectDataSource(5)]List<Project> entities)
        {
            return base.AddRangeTest_GoodFlow(entities);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override void AddTest_BadFlow_Null()
        {
            base.AddTest_BadFlow_Null();
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public async Task FindAsyncTest_BadFlow_NotExists(
            [ProjectDataSource]Project project,
            [CollaboratorDataSource(10)]List<Collaborator> collaborators)
        {
            project.Collaborators = collaborators;
            DbContext.Add(project);
            await DbContext.SaveChangesAsync();

            Assert.IsNull(await Repository.FindAsync(-1));
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public async Task FindAsyncTest_GoodFlow([ProjectDataSource]Project project,
            [CollaboratorDataSource(10)]List<Collaborator> collaborators)
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

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task GetAllAsyncTest_Badflow_Empty()
        {
            return base.GetAllAsyncTest_Badflow_Empty();
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task GetAllAsyncTest_GoodFlow([ProjectDataSource(5)]List<Project> entities)
        {
            return base.GetAllAsyncTest_GoodFlow(entities);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task RemoveAsyncTest_BadFlow_NotExists([ProjectDataSource]Project entity)
        {
            return base.RemoveAsyncTest_BadFlow_NotExists(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task RemoveAsyncTest_GoodFlow([ProjectDataSource]Project entity)
        {
            return base.RemoveAsyncTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task UpdateTest_BadFlow_NotExists([ProjectDataSource]Project entity, [ProjectDataSource]Project updateEntity)
        {
            return base.UpdateTest_BadFlow_NotExists(entity, updateEntity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task UpdateTest_BadFlow_Null([ProjectDataSource]Project entity)
        {
            return base.UpdateTest_BadFlow_Null(entity);
        }

        ///<inheritdoc cref="RepositoryTest{TDomain, TRepository}"/>
        [Test]
        public override Task UpdateTest_GoodFlow([ProjectDataSource]Project entity, [ProjectDataSource]Project updateEntity)
        {
            return base.UpdateTest_GoodFlow(entity, updateEntity);
        }

        private List<Project> SetStaticTestData(List<Project> projects, List<User> users = null)
        {
            for (int i = 0; i < projects.Count; i++)
            {
                projects[i].Name = "exName";
                projects[i].Description = "";
                projects[i].ShortDescription = "";
                projects[i].Uri = "";
                if (users != null)
                {
                    projects[i].User = users[i];
                    projects[i].User.Name = "";
                }
            }

            for (int i = 0; i < 50; i++)
            {
                projects[i].Name = "abcex";
                projects[i].Description = "defex";
                projects[i].ShortDescription = "ghijex";
                projects[i].Uri = "https://www.example.com/";
                if (users != null)
                {
                    projects[i].User.Name = "xyzex";
                }
            }

            return projects;
        }
    }
}
