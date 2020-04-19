using _3_Repositories.Tests.Base;
using _3_Repositories.Tests.DataSources;
using Models;
using NUnit.Framework;
using Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _3_Repositories.Tests
{
    [TestFixture]
    public class ProjectRepositoryTest : RepositoryTest<Project, ProjectRepository>
    {
        protected new IProjectRepository _repository => (IProjectRepository)base._repository;

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
            _dbContext.AddRange(projects);
            await SaveChangesAsync();

            // Test
            List<Project> retrieved = await _repository.GetAllWithUsersAsync();
            Assert.AreEqual(100, retrieved.Count);
            foreach(Project project in projects)
            {
                Assert.IsNotNull(project.User);
            }
        }

        [Test]
        public async Task GetAllWithUsersAsyncTest_NoProjects([UserDataSource(100)]List<User> users)
        {
            _dbContext.AddRange(users);
            await SaveChangesAsync();

            // Test
            List<Project> retrieved = await _repository.GetAllWithUsersAsync();
            Assert.AreEqual(0, retrieved.Count);
        }

        [Test]
        public async Task GetAllWithUsersAsyncTest_NoUsers(
            [ProjectDataSource(100)]List<Project> projects)
        {
            // Seed database
            _dbContext.AddRange(projects);
            await SaveChangesAsync();

            // Test
            List<Project> retrieved = await _repository.GetAllWithUsersAsync();
            Assert.AreEqual(0, retrieved.Count);
            foreach (Project project in projects)
            {
                Assert.IsNull(project.User);
            }
        }

        [Test]
        public async Task SearchAsyncTest_GoodFlow(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            _dbContext.AddRange(projects);
            await SaveChangesAsync();

            // Tests
            // Id search
            List<Project> retrieved = (List<Project>)await _repository.SearchAsync("1");
            foreach (Project project in retrieved)
            {
                Assert.True(project.Id.ToString().Contains("1"), "Id search failed");
            }

            // Name search
            retrieved = (List<Project>)await _repository.SearchAsync("abc");
            Assert.AreEqual(50, retrieved.Count, "Name search failed");

            // Description search
            retrieved = (List<Project>)await _repository.SearchAsync("def");
            Assert.AreEqual(50, retrieved.Count, "Description search failed");

            // Short Description search
            retrieved = (List<Project>)await _repository.SearchAsync("ghij");
            Assert.AreEqual(50, retrieved.Count, "ShortDescription search failed");

            // Uri search
            retrieved = (List<Project>)await _repository.SearchAsync("example");
            Assert.AreEqual(50, retrieved.Count, "Uri search failed");

            // User name search
            retrieved = (List<Project>)await _repository.SearchAsync("xyz");
            Assert.AreEqual(50, retrieved.Count, "User name search failed");

            // Combined search
            retrieved = (List<Project>)await _repository.SearchAsync("ex");
            Assert.AreEqual(100, retrieved.Count, "Combined search failed");
        }

        [Test]
        public async Task SearchAsyncTest_BadFlow_NoProjects(
            [UserDataSource(100)]List<User> users)
        {
            // Seed database
            _dbContext.AddRange(users);
            await SaveChangesAsync();

            // Test
            List<Project> retrieved = (List<Project>)await _repository.SearchAsync("abc");
            Assert.AreEqual(0, retrieved.Count);
        }

        [Test]
        public async Task SearchAsyncTest_BadFlow_NoUsers([ProjectDataSource(100)]List<Project> projects)
        {
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects);

            _dbContext.AddRange(projects);
            await SaveChangesAsync();
        }

        [Test]
        public async Task SearchAsyncTest_BadFlow_NoMatching(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            _dbContext.AddRange(projects);
            await SaveChangesAsync();

            // Tests
            // Id search
            List<Project> retrieved = (List<Project>)await _repository.SearchAsync("-1");
            Assert.AreEqual(0, retrieved.Count);
        }

        [Test]
        public async Task SearchSkipTakeAsyncTest_GoodFlow(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            _dbContext.AddRange(projects);
            await SaveChangesAsync();

            // Tests
            // Id search
            List<Project> retrieved = (List<Project>)await _repository.SearchAsync("1", 0, 1);
            Assert.AreEqual(1, retrieved.Count, "Id search failed");

            // Name search
            retrieved = (List<Project>)await _repository.SearchAsync("abc", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "Name search failed");

            // Description search
            retrieved = (List<Project>)await _repository.SearchAsync("def", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "Description search failed");

            // Short Description search
            retrieved = (List<Project>)await _repository.SearchAsync("ghij", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "Short Description search failed");

            // Uri search
            retrieved = (List<Project>)await _repository.SearchAsync("example", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "Uri search failed");

            // User name search
            retrieved = (List<Project>)await _repository.SearchAsync("xyz", 10, 10);
            Assert.AreEqual(10, retrieved.Count, "User name search failed");

            // Combined search
            retrieved = (List<Project>)await _repository.SearchAsync("ex", 10, 40);
            Assert.AreEqual(40, retrieved.Count, "Combined search failed");
        }

        [Test]
        public async Task SearchSkipTakeAsyncTest_BadFlow_SkipAllProjects(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            _dbContext.AddRange(projects);
            await SaveChangesAsync();

            // Tests
            List<Project> retrieved = (List<Project>)await _repository.SearchAsync("ex", 1000, 10);
            Assert.AreEqual(0, retrieved.Count);
        }

        [Test]
        public async Task SearchCountAsyncTest_GoodFlow(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            _dbContext.AddRange(projects);
            await SaveChangesAsync();

            // Tests
            int retrieved = await _repository.SearchCountAsync("ex");
            Assert.AreEqual(100, retrieved);
        }

        [Test]
        public async Task SearchCountAsyncTest_BadFlow_NoMatchingProjects(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Set project - user relation
            // Set some properties to be able to search
            // And seed database
            projects = SetStaticTestData(projects, users);

            _dbContext.AddRange(projects);
            await SaveChangesAsync();

            // Tests
            int retrieved = await _repository.SearchCountAsync("randomSearchWhichDoesntMatch");
            Assert.AreEqual(0, retrieved);
        }

        [Test]
        public async Task SearchCountAsyncTest_BadFlow_NoProjects()
        {
            // Tests
            int retrieved = await _repository.SearchCountAsync("1");
            Assert.AreEqual(0, retrieved);
        }

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

            _dbContext.AddRange(projects);
            await SaveChangesAsync();

            // Testing
            Project retrieved = await _repository.FindWithUserAndCollaboratorsAsync(1);
            Assert.AreEqual(projects[0].Id, retrieved.Id);
            Assert.AreEqual(projects[0].Name, retrieved.Name);
            Assert.AreEqual(projects[0].ShortDescription, retrieved.ShortDescription);
            Assert.AreEqual(projects[0].Description, retrieved.Description);
            Assert.AreEqual(projects[0].Uri, retrieved.Uri);
            Assert.AreEqual(projects[0].User.Name, retrieved.User.Name);
            Assert.AreEqual(projects[0].Collaborators[0].FullName, retrieved.Collaborators[0].FullName);
        }

        [Test]
        public async Task FindWithUserAndCollaboratorsAsyncTest_BadFlow_NoProjects()
        {
            // Testing
            Project retrieved = await _repository.FindWithUserAndCollaboratorsAsync(1);
            Assert.IsNull(retrieved);
        }

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

            _dbContext.AddRange(projects);
            await SaveChangesAsync();

            // Testing
            Project retrieved = await _repository.FindWithUserAndCollaboratorsAsync(1);
            
            // Retrieved is null because there are no users and users is required
            Assert.IsNull(retrieved);
        }

        [Test]
        public async Task FindWithUserAndCollaboratorsAsyncTest_BadFlow_NoCollaborators(
            [ProjectDataSource(100)]List<Project> projects,
            [UserDataSource(100)]List<User> users)
        {
            // Seeding
            projects = SetStaticTestData(projects, users);
            
            _dbContext.AddRange(projects);
            await SaveChangesAsync();

            // Testing
            Project retrieved = await _repository.FindWithUserAndCollaboratorsAsync(1);
            Assert.AreEqual(projects[0].Id, retrieved.Id);
            Assert.AreEqual(projects[0].Name, retrieved.Name);
            Assert.AreEqual(projects[0].ShortDescription, retrieved.ShortDescription);
            Assert.AreEqual(projects[0].Description, retrieved.Description);
            Assert.AreEqual(projects[0].Uri, retrieved.Uri);
            Assert.AreEqual(projects[0].User.Name, retrieved.User.Name);
            Assert.AreEqual(0, retrieved.Collaborators.Count);
        }


        //Default test from RepositoryTest
        [Test]
        public override Task AddAsyncTest_GoodFlow([ProjectDataSource]Project entity)
        {
            return base.AddAsyncTest_GoodFlow(entity);
        }

        [Test]
        public override void AddRangeTest_BadFlow_EmptyList()
        {
            base.AddRangeTest_BadFlow_EmptyList();
        }

        [Test]
        public override void AddRangeTest_BadFlow_Null()
        {
            base.AddRangeTest_BadFlow_Null();
        }

        [Test]
        public override Task AddRangeTest_GoodFlow([ProjectDataSource(5)]List<Project> entities)
        {
            return base.AddRangeTest_GoodFlow(entities);
        }

        [Test]
        public override void AddTest_BadFlow_Null()
        {
            base.AddTest_BadFlow_Null();
        }

        // Override default test with extra parameters due to override in repository
        [Test]
        public async Task FindAsyncTest_BadFlow_NotExists(
            [ProjectDataSource]Project project,
            [CollaboratorDataSource(10)]List<Collaborator> collaborators)
        {
            project.Collaborators = collaborators;
            _dbContext.Add(project);
            await SaveChangesAsync();

            Assert.IsNull(await _repository.FindAsync(-1));
        }

        // Override default test with extra parameters due to override in repository
        [Test]
        public async Task FindAsyncTest_GoodFlow([ProjectDataSource]Project project,
            [CollaboratorDataSource(10)]List<Collaborator> collaborators)
        {
            project.Collaborators = collaborators;
            _dbContext.Add(project);
            await SaveChangesAsync();

            Project retrieved = await _repository.FindAsync(1);
            Assert.AreEqual(project.Id, retrieved.Id);
            Assert.AreEqual(project.Name, retrieved.Name);
            Assert.AreEqual(project.Description, retrieved.Description);
            Assert.AreEqual(project.ShortDescription, retrieved.ShortDescription);
            Assert.AreEqual(project.Uri, retrieved.Uri);
            Assert.AreEqual(project.Collaborators.Count, retrieved.Collaborators.Count);
        }

        [Test]
        public override Task GetAllAsyncTest_Badflow_Empty()
        {
            return base.GetAllAsyncTest_Badflow_Empty();
        }

        [Test]
        public Task GetAllAsyncTest_GoodFlow([ProjectDataSource(5)]List<Project> entities)
        {
            return base.GetAllAsyncTest_GoodFlow(entities, 5);
        }

        [Test]
        public override Task RemoveAsyncTest_BadFlow_NotExists([ProjectDataSource]Project entity)
        {
            return base.RemoveAsyncTest_BadFlow_NotExists(entity);
        }

        [Test]
        public override Task RemoveAsyncTest_GoodFlow([ProjectDataSource]Project entity)
        {
            return base.RemoveAsyncTest_GoodFlow(entity);
        }

        [Test]
        public override Task UpdateTest_BadFlow_NotExists([ProjectDataSource]Project entity, [ProjectDataSource]Project updateEntity)
        {
            return base.UpdateTest_BadFlow_NotExists(entity, updateEntity);
        }

        [Test]
        public override Task UpdateTest_BadFlow_Null([ProjectDataSource]Project entity)
        {
            return base.UpdateTest_BadFlow_Null(entity);
        }

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
