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

        protected new IProjectService Service => base.Service;


        /// <summary>
        ///     Test whether the repository method is called and no changes have been applied to the object
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllWithUsersAsync_GoodFlow([ProjectDataSource(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.GetAllWithUsersCollaboratorsAndInstitutionsAsync(
                                         null,
                                         null,
                                         project => project.Updated,
                                         true,
                                         null,
                                         null))
                          .Returns(Task.FromResult(projects));

            List<Project> retrievedProjects = await Service.GetAllWithUsersCollaboratorsAndInstitutionsAsync(new ProjectFilterParams
                                                  {
                                                      Page = null,
                                                      AmountOnPage = null,
                                                      Highlighted = null,
                                                      SortBy = null,
                                                      SortDirection = "asc"
                                                  });

            Assert.DoesNotThrow(() =>
            {
                RepositoryMock.Verify(repository =>
                                          repository.GetAllWithUsersCollaboratorsAndInstitutionsAsync(
                                              null,
                                              null,
                                              project => project.Updated,
                                              true,
                                              null,
                                              null),
                                      Times.Once);
            });

            Assert.AreEqual(projects, retrievedProjects);
        }

        /// <summary>
        ///     Test whether the repository method is called with the orderBy created parameter and ordering ascending and no
        ///     changes have been applied to the projects
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllOrderedByCreatedAscendingAsync_GoodFlow([ProjectDataSource(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.GetAllWithUsersCollaboratorsAndInstitutionsAsync(
                                         null,
                                         null,
                                         project => project.Created,
                                         true,
                                         null,
                                         null))
                          .Returns(Task.FromResult(projects));

            List<Project> retrievedProjects = await Service.GetAllWithUsersCollaboratorsAndInstitutionsAsync(new ProjectFilterParams
                                                  {
                                                      Page = null,
                                                      AmountOnPage = null,
                                                      Highlighted = null,
                                                      SortBy = "created",
                                                      SortDirection = "asc"
                                                  });

            Assert.DoesNotThrow(() =>
            {
                RepositoryMock.Verify(repository =>
                                          repository.GetAllWithUsersCollaboratorsAndInstitutionsAsync(
                                              null,
                                              null,
                                              project => project.Created,
                                              true,
                                              null,
                                              null),
                                      Times.Once);
            });

            Assert.AreEqual(projects, retrievedProjects);
        }

        /// <summary>
        ///     Test whether the repository method is called with the orderBy name parameter and ordering descending and no changes
        ///     have been applied to the projects
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllOrderedByNameDescendingAsync_GoodFlow([ProjectDataSource(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.GetAllWithUsersCollaboratorsAndInstitutionsAsync(
                                         null,
                                         null,
                                         project => project.Name,
                                         false,
                                         null,
                                         null))
                          .Returns(Task.FromResult(projects));

            List<Project> retrievedProjects = await Service.GetAllWithUsersCollaboratorsAndInstitutionsAsync(new ProjectFilterParams
                                                  {
                                                      Page = null,
                                                      AmountOnPage = null,
                                                      Highlighted = null,
                                                      SortBy = "name",
                                                      SortDirection = "desc"
                                                  });

            Assert.DoesNotThrow(() =>
            {
                RepositoryMock.Verify(repository =>
                                          repository.GetAllWithUsersCollaboratorsAndInstitutionsAsync(
                                              null,
                                              null,
                                              project => project.Name,
                                              false,
                                              null,
                                              null),
                                      Times.Once);
            });

            Assert.AreEqual(projects, retrievedProjects);
        }

        /// <summary>
        ///     Test whether the repository method is called with the highlighted filter and no changes have been applied to the
        ///     projects
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllHighlightedAsync_GoodFlow([ProjectDataSource(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.GetAllWithUsersCollaboratorsAndInstitutionsAsync(
                                         null,
                                         null,
                                         project => project.Updated,
                                         true,
                                         true,
                                         null))
                          .Returns(Task.FromResult(projects));

            List<Project> retrievedProjects = await Service.GetAllWithUsersCollaboratorsAndInstitutionsAsync(new ProjectFilterParams
                                                  {
                                                      Page = null,
                                                      AmountOnPage = null,
                                                      Highlighted = true,
                                                      SortBy = null,
                                                      SortDirection = "asc"
                                                  });

            Assert.DoesNotThrow(() =>
            {
                RepositoryMock.Verify(repository =>
                                          repository.GetAllWithUsersCollaboratorsAndInstitutionsAsync(
                                              null,
                                              null,
                                              project => project.Updated,
                                              true,
                                              true,
                                              null),
                                      Times.Once);
            });

            Assert.AreEqual(projects, retrievedProjects);
        }

        /// <summary>
        ///     Test whether the repository method is called with the no highlighted filter and no changes have been applied to the
        ///     projects
        /// </summary>
        /// <param name="projects">The projects which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task GetAllNoHighlightedAsync_GoodFlow([ProjectDataSource(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.GetAllWithUsersCollaboratorsAndInstitutionsAsync(
                                         null,
                                         null,
                                         project => project.Updated,
                                         true,
                                         false,
                                         null))
                          .Returns(Task.FromResult(projects));

            List<Project> retrievedProjects = await Service.GetAllWithUsersCollaboratorsAndInstitutionsAsync(new ProjectFilterParams
                                                  {
                                                      Page = null,
                                                      AmountOnPage = null,
                                                      Highlighted = false,
                                                      SortBy = null,
                                                      SortDirection = "asc"
                                                  });

            Assert.DoesNotThrow(() =>
            {
                RepositoryMock.Verify(repository =>
                                          repository.GetAllWithUsersCollaboratorsAndInstitutionsAsync(
                                              null,
                                              null,
                                              project => project.Updated,
                                              true,
                                              false,
                                              null),
                                      Times.Once);
            });

            Assert.AreEqual(projects, retrievedProjects);
        }

        /// <summary>
        ///     Test whether the repository method is called and no changes are applied to the object
        /// </summary>
        /// <param name="project">The projects which are used as data to test</param>
        /// <returns></returns>
        [Test]
        public async Task FindWithUserAndCollaboratorsAsync([ProjectDataSource] Project project)
        {
            RepositoryMock.Setup(repository => repository.FindWithUserCollaboratorsAndInstitutionsAsync(1))
                          .Returns(Task.FromResult(project));

            Project retrievedProject = await Service.FindWithUserCollaboratorsAndInstitutionsAsync(1);

            Assert.DoesNotThrow(() =>
            {
                RepositoryMock.Verify(repository => repository.FindWithUserCollaboratorsAndInstitutionsAsync(1),
                                      Times.Once);
            });

            Assert.AreEqual(project, retrievedProject);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void AddRangeTest_GoodFlow([ProjectDataSource(100)] IEnumerable<Project> entities)
        {
            base.AddRangeTest_GoodFlow(entities);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void AddTest_GoodFlow([ProjectDataSource] Project entity)
        {
            base.AddTest_GoodFlow(entity);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override Task GetAll([ProjectDataSource(100)] List<Project> entities)
        {
            return base.GetAll(entities);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void Remove([ProjectDataSource] Project entity)
        {
            base.Remove(entity);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public Task RemoveAsync()
        {
            return base.RemoveAsync(1);
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void Save()
        {
            base.Save();
        }

        /// <inheritdoc cref="ServiceTest{TDomain, TService, TRepository}" />
        [Test]
        public override void Update([ProjectDataSource] Project entity)
        {
            base.Update(entity);
        }

        /// <summary>
        ///     Test wheter the save method removes unsanitary strings from the description.
        /// </summary>
        [Test]
        public void SaveSanitizationInvalidDescription([ProjectDataSource] Project project)
        {
            string descriptionToSanitize = "<script>alert(1)</script>";
            string descriptionAfterSanitization = "";

            project.Description = descriptionToSanitize;
            Service.Add(project);

            Assert.AreEqual(descriptionAfterSanitization, project.Description);
        }

        /// <summary>
        ///     Test wheter the save method does not remove sanitary strings from the description.
        /// </summary>
        [Test]
        public void SaveSanitizationValidDescription([ProjectDataSource] Project project)
        {
            string descriptionToSanitize = "<p>test</p>";

            project.Description = descriptionToSanitize;
            Service.Add(project);

            Assert.AreEqual(descriptionToSanitize, project.Description);
        }

        /// <summary>
        ///     Test wheter the update method removes unsanitary strings from the description.
        /// </summary>
        [Test]
        public void UpdateSanitizationInvalidDescription([ProjectDataSource] Project project)
        {
            string descriptionToSanitize = "<script>alert(1)</script>";
            string descriptionAfterSanitization = "";

            project.Description = descriptionToSanitize;
            Service.Update(project);

            Assert.AreEqual(descriptionAfterSanitization, project.Description);
        }

        /// <summary>
        ///     Test wheter the update method does not remove sanitary strings from the description.
        /// </summary>
        [Test]
        public void UpdateSanitizationValidDescription([ProjectDataSource] Project project)
        {
            string descriptionToSanitize = "<p>test</p>";

            project.Description = descriptionToSanitize;
            Service.Update(project);

            Assert.AreEqual(descriptionToSanitize, project.Description);
        }

    }

}
