using Models;
using Moq;
using NUnit.Framework;
using Repositories;
using Repositories.Tests.DataSources;
using Services.Services;
using Services.Tests.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Tests
{
    [TestFixture]
    public class ProjectServiceTest : ServiceTest<Project, ProjectService, IProjectRepository>
    {
        protected new IProjectService Service => (IProjectService)base.Service;


        [Test]
        public async Task GetAllWithUsersAsync_GoodFlow([ProjectDataSource(10)]List<Project> projects)
        {
            RepositoryMock.Setup(
                repository => repository.GetAllWithUsersAsync())
                .Returns(
                    Task.FromResult(projects)
                );

            List<Project> retrievedProjects = await Service.GetAllWithUsersAsync();

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.GetAllWithUsersAsync(), Times.Once);
            });

            Assert.AreEqual(projects, retrievedProjects);
        }

        [Test]
        public async Task FindWithUserAndCollaboratorsAsync([ProjectDataSource]Project project)
        {
            RepositoryMock.Setup(
               repository => repository.FindWithUserAndCollaboratorsAsync(1))
               .Returns(
                   Task.FromResult(project)
               );

            Project retrievedProject = await Service.FindWithUserAndCollaboratorsAsync(1);

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.FindWithUserAndCollaboratorsAsync(1), Times.Once);
            });

            Assert.AreEqual(project, retrievedProject);
        }

        // Default tests from base class
        // Calling base method with correct parameters to trigger default test
        [Test]
        public override void AddRangeTest_GoodFlow([ProjectDataSource(100)]IEnumerable<Project> entities)
        {
            base.AddRangeTest_GoodFlow(entities);
        }

        // Calling base method with correct parameters to trigger default test
        [Test]
        public override void AddTest_GoodFlow([ProjectDataSource]Project entity)
        {
            base.AddTest_GoodFlow(entity);
        }

        // Calling base method with correct parameters to trigger default test
        [Test]
        public Task GetAll([ProjectDataSource(100)]List<Project> entities)
        {
            return base.GetAll(entities, 100);
        }

        // Calling base method with correct parameters to trigger default test
        [Test]
        public override void Remove([ProjectDataSource]Project entity)
        {
            base.Remove(entity);
        }

        // Calling base method with correct parameters to trigger default test
        [Test]
        public Task RemoveAsync()
        {
            return base.RemoveAsync(1);
        }

        // Calling base method with correct parameters to trigger default test
        [Test]
        public override void Save()
        {
            base.Save();
        }

        // Calling base method with correct parameters to trigger default test
        [Test]
        public override void Update([ProjectDataSource]Project entity)
        {
            base.Update(entity);
        }
    }
}
