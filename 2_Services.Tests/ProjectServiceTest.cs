using _2_Services.Tests.Base;
using _3_Repositories.Tests.DataSources;
using Models;
using Moq;
using NUnit.Framework;
using Repositories;
using Services.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _2_Services.Tests
{
    [TestFixture]
    public class ProjectServiceTest : ServiceTest<Project, ProjectService, IProjectRepository>
    {
        protected new IProjectService _service => (IProjectService)base._service;


        [Test]
        public async Task GetAllWithUsersAsync_GoodFlow([ProjectDataSource(10)]List<Project> projects)
        {
            _repositoryMock.Setup(
                repository => repository.GetAllWithUsersAsync())
                .Returns(
                    Task.FromResult(projects)
                );

            List<Project> retrievedProjects = await _service.GetAllWithUsersAsync();

            Assert.DoesNotThrow(() => {
                _repositoryMock.Verify(repository => repository.GetAllWithUsersAsync(), Times.Once);
            });

            Assert.AreEqual(projects, retrievedProjects);
        }

        [Test]
        public async Task FindWithUserAndCollaboratorsAsync([ProjectDataSource]Project project)
        {
            _repositoryMock.Setup(
               repository => repository.FindWithUserAndCollaboratorsAsync(1))
               .Returns(
                   Task.FromResult(project)
               );

            Project retrievedProject = await _service.FindWithUserAndCollaboratorsAsync(1);

            Assert.DoesNotThrow(() => {
                _repositoryMock.Verify(repository => repository.FindWithUserAndCollaboratorsAsync(1), Times.Once);
            });

            Assert.AreEqual(project, retrievedProject);
        }

        // Default tests from base class
        [Test]
        public override void AddRangeTest_GoodFlow([ProjectDataSource(100)]IEnumerable<Project> entities)
        {
            base.AddRangeTest_GoodFlow(entities);
        }

        [Test]
        public override void AddTest_GoodFlow([ProjectDataSource]Project entity)
        {
            base.AddTest_GoodFlow(entity);
        }

        [Test]
        public Task GetAll([ProjectDataSource(100)]List<Project> entities)
        {
            return base.GetAll(entities, 100);
        }

        [Test]
        public override void Remove([ProjectDataSource]Project entity)
        {
            base.Remove(entity);
        }

        [Test]
        public Task RemoveAsync()
        {
            return base.RemoveAsync(1);
        }

        [Test]
        public override void Save()
        {
            base.Save();
        }

        [Test]
        public override void Update([ProjectDataSource]Project entity)
        {
            base.Update(entity);
        }
    }
}