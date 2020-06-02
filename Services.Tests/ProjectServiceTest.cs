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


        /// <summary>
        /// Test whether the repository method is called and no changes have been applied to the object
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllWithUsersAsync_GoodFlow([ProjectDataSource(10)]List<Project> projects)
        {
            RepositoryMock.Setup(
                repository => repository.GetAllWithUsersAsync(null, null, project => project.Updated, true, null))
                .Returns(
                    Task.FromResult(projects)
                );

            List<Project> retrievedProjects = await Service.GetAllWithUsersAsync(new ProjectFilterParams()
                                                                                 {
                                                                                     Page = null,
                                                                                     AmountOnPage = null,
                                                                                     Highlighted = null,
                                                                                     SortBy = null,
                                                                                     SortDirection = "asc"
                                                                                 });

            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.GetAllWithUsersAsync(null, null, project => project.Updated, true, null), Times.Once);
            });

            Assert.AreEqual(projects, retrievedProjects);
        }

        /// <summary>
        /// Test whether the repository method is called and no changes are applied to the object
        /// </summary>
        /// <param name="project">The projects which are used as data to test</param>
        /// <returns></returns>
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

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void AddRangeTest_GoodFlow([ProjectDataSource(100)]IEnumerable<Project> entities)
        {
            base.AddRangeTest_GoodFlow(entities);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void AddTest_GoodFlow([ProjectDataSource]Project entity)
        {
            base.AddTest_GoodFlow(entity);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override Task GetAll([ProjectDataSource(100)]List<Project> entities)
        {
            return base.GetAll(entities);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void Remove([ProjectDataSource]Project entity)
        {
            base.Remove(entity);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public Task RemoveAsync()
        {
            return base.RemoveAsync(1);
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void Save()
        {
            base.Save();
        }

        ///<inheritdoc cref="ServiceTest{TDomain, TService, TRepository}"/>
        [Test]
        public override void Update([ProjectDataSource]Project entity)
        {
            base.Update(entity);
        }
    }
}
