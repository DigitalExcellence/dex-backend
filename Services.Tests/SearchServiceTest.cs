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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Tests
{
    /// <summary>
    /// SearchServiceTest
    /// </summary>
    /// <seealso cref="IProjectRepository" />
    [TestFixture]
    public class SearchServiceTest
    {
        protected ISearchService Service;
        protected Mock<IProjectRepository> RepositoryMock;

        /// <summary>
        /// Initialize runs before every test
        /// Mock the repository
        /// Initialize the service with reflection
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            // Mock the repository
            RepositoryMock = new Mock<IProjectRepository>();
            Service = new SearchService(RepositoryMock.Object);
        }

        /// <summary>
        /// Test whether the repository method is called with sortDirection set to asc and no changes are applied to the object.
        /// </summary>
        /// <param name="projects">The projects which are used as data to test.</param>
        [Test]
        public async Task SearchInternalProjects_goodflow_sort_direction_true([ProjectDataSourceAttribute(10)]List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.SearchAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<Project, object>>>(), It.IsAny<bool>(), It.IsAny<bool?>()))
                          .Returns(Task.FromResult(projects.AsEnumerable()));

            IEnumerable<Project> resultedProjects = await Service.SearchInternalProjects("",
                                                                            new ProjectFilterParams()
                                                                            {
                                                                                AmountOnPage = null,
                                                                                Highlighted = null,
                                                                                Page = null,
                                                                                SortBy = null,
                                                                                SortDirection = "asc",
                                                                            });
            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.SearchAsync("", null, null,project => project.Updated, true, null), Times.Once);
            });

            Assert.AreEqual(projects.AsEnumerable(), resultedProjects);
        }

        /// <summary>
        /// Test whether the repository method is called with sortDirection set to descending and no changes are applied to the object.
        /// </summary>
        /// <param name="projects">The projects which are used as data to test..</param>
        [Test]
        public async Task SearchInternalProjects_goodflow_sort_direction_false([ProjectDataSourceAttribute(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.SearchAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<Project, object>>>(), It.IsAny<bool>(), It.IsAny<bool?>()))
                          .Returns(Task.FromResult(projects.AsEnumerable()));

            IEnumerable<Project> resultedProjects = await Service.SearchInternalProjects("",
                                                                                         new ProjectFilterParams()
                                                                                         {
                                                                                             AmountOnPage = null,
                                                                                             Highlighted = null,
                                                                                             Page = null,
                                                                                             SortBy = null,
                                                                                             SortDirection = "desc",
                                                                                         });
            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.SearchAsync("", null, null, project => project.Updated, false, null), Times.Once);
            });

            Assert.AreEqual(projects.AsEnumerable(), resultedProjects);
        }

        /// <summary>
        /// Test whether the repository method is called with sortBy set to name and no changes are applied to the object.
        /// </summary>
        /// <param name="projects">The projects which are used as data to test..</param>
        [Test]
        public async Task SearchInternalProjects_goodflow_sortBy_name([ProjectDataSourceAttribute(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.SearchAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<Project, object>>>(), It.IsAny<bool>(), It.IsAny<bool?>()))
                          .Returns(Task.FromResult(projects.AsEnumerable()));

            IEnumerable<Project> resultedProjects = await Service.SearchInternalProjects("",
                                                                                         new ProjectFilterParams()
                                                                                         {
                                                                                             AmountOnPage = null,
                                                                                             Highlighted = null,
                                                                                             Page = null,
                                                                                             SortBy = "name",
                                                                                             SortDirection = "desc",
                                                                                         });
            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.SearchAsync("", null, null, project => project.Name, false, null), Times.Once);
            });

            Assert.AreEqual(projects.AsEnumerable(), resultedProjects);
        }

        /// <summary>
        /// Test whether the repository method is called with sortBy set to created and no changes are applied to the object.
        /// </summary>
        /// <param name="projects">The projects which are used as data to test..</param>
        [Test]
        public async Task SearchInternalProjects_goodflow_sortBy_Created([ProjectDataSourceAttribute(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.SearchAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<Project, object>>>(), It.IsAny<bool>(), It.IsAny<bool?>()))
                          .Returns(Task.FromResult(projects.AsEnumerable()));

            IEnumerable<Project> resultedProjects = await Service.SearchInternalProjects("",
                                                                                         new ProjectFilterParams()
                                                                                         {
                                                                                             AmountOnPage = null,
                                                                                             Highlighted = null,
                                                                                             Page = null,
                                                                                             SortBy = "created",
                                                                                             SortDirection = "desc",
                                                                                         });
            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.SearchAsync("", null, null, project => project.Created, false, null), Times.Once);
            });

            Assert.AreEqual(projects.AsEnumerable(), resultedProjects);
        }

        /// <summary>
        /// Test whether the repository method is called with amountOnPage set to null no changes are applied to the object.
        /// </summary>
        /// <param name="projects">The projects which are used as data to test..</param>
        [Test]
        public async Task SearchInternalProjects_goodflow_Amount_on_Page([ProjectDataSourceAttribute(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.SearchAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<Project, object>>>(), It.IsAny<bool>(), It.IsAny<bool?>()))
                          .Returns(Task.FromResult(projects.AsEnumerable()));

            IEnumerable<Project> resultedProjects = await Service.SearchInternalProjects("",
                                                                                         new ProjectFilterParams()
                                                                                         {
                                                                                             AmountOnPage = null,
                                                                                             Highlighted = null,
                                                                                             Page = 1,
                                                                                             SortBy = null,
                                                                                             SortDirection = "asc",
                                                                                         });
            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.SearchAsync("", 0, 20, project => project.Updated, true, null), Times.Once);
            });

            Assert.AreEqual(projects.AsEnumerable(), resultedProjects);
        }

        /// <summary>
        /// Test whether the repository method is called with amountOnPage set to a custom value and no changes are applied to the object.
        /// </summary>
        /// <param name="projects">The projects which are used as data to test..</param>
        [Test]
        public async Task SearchInternalProjects_goodflow_Amount_on_Page_custom([ProjectDataSourceAttribute(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.SearchAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<Project, object>>>(), It.IsAny<bool>(), It.IsAny<bool?>()))
                          .Returns(Task.FromResult(projects.AsEnumerable()));

            IEnumerable<Project> resultedProjects = await Service.SearchInternalProjects("",
                                                                                         new ProjectFilterParams()
                                                                                         {
                                                                                             AmountOnPage = 17,
                                                                                             Highlighted = null,
                                                                                             Page = 2,
                                                                                             SortBy = null,
                                                                                             SortDirection = "asc",
                                                                                         });
            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.SearchAsync("", 17, 17, project => project.Updated, true, null), Times.Once);
            });

            Assert.AreEqual(projects.AsEnumerable(), resultedProjects);
        }

        /// <summary>
        /// Test whether the repository method is called with highlighted filter set to null and no changes are applied to the object.
        /// </summary>
        /// <param name="projects">The projects which are used as data to test..</param>
        [Test]
        public async Task SearchInternalProjectsCount_highlighted_null([ProjectDataSourceAttribute(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.SearchCountAsync(It.IsAny<string>(), It.IsAny<bool?>()))
                          .Returns(Task.FromResult(projects.Count));

            int totalCount = await Service.SearchInternalProjectsCount("",
                                                                                         new ProjectFilterParams()
                                                                                         {
                                                                                             AmountOnPage = null,
                                                                                             Highlighted = null,
                                                                                             Page = null,
                                                                                             SortBy = null,
                                                                                             SortDirection = "asc",
                                                                                         });
            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.SearchCountAsync("", null), Times.Once);
            });

            Assert.AreEqual(projects.Count, totalCount);
        }

        /// <summary>
        /// Test whether the repository method is called with highlighted filter set to true and no changes are applied to the object.
        /// </summary>
        /// <param name="projects">The projects which are used as data to test..</param>
        [Test]
        public async Task SearchInternalProjectsCount_highlighted_true([ProjectDataSourceAttribute(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.SearchCountAsync(It.IsAny<string>(), It.IsAny<bool?>()))
                          .Returns(Task.FromResult(projects.Count));

            int totalCount = await Service.SearchInternalProjectsCount("",
                                                                                         new ProjectFilterParams()
                                                                                         {
                                                                                             AmountOnPage = null,
                                                                                             Highlighted = true,
                                                                                             Page = null,
                                                                                             SortBy = null,
                                                                                             SortDirection = "asc",
                                                                                         });
            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.SearchCountAsync("", true), Times.Once);
            });

            Assert.AreEqual(projects.Count, totalCount);
        }

        /// <summary>
        /// Test whether the repository method is called with highlighted filter set to false and no changes are applied to the object.
        /// </summary>
        /// <param name="projects">The projects which are used as data to test..</param>
        [Test]
        public async Task SearchInternalProjectsCount_highlighted_false([ProjectDataSourceAttribute(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.SearchCountAsync(It.IsAny<string>(), It.IsAny<bool?>()))
                          .Returns(Task.FromResult(projects.Count));

            int totalCount = await Service.SearchInternalProjectsCount("",
                                                                                         new ProjectFilterParams()
                                                                                         {
                                                                                             AmountOnPage = null,
                                                                                             Highlighted = false,
                                                                                             Page = null,
                                                                                             SortBy = null,
                                                                                             SortDirection = "asc",
                                                                                         });
            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.SearchCountAsync("", false), Times.Once);
            });

            Assert.AreEqual(projects.Count, totalCount);
        }

        /// <summary>
        /// Test whether the repository method is called with a custom amount filter and no changes are applied to the object.
        /// </summary>
        /// <param name="projects">The projects which are used as data to test..</param>
        [Test]
        public async Task SearchInternalProjectsTotalPages_goodflow_custom_amountOnPage([ProjectDataSourceAttribute(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.SearchCountAsync(It.IsAny<string>(), It.IsAny<bool?>()))
                          .Returns(Task.FromResult(projects.Count));

            int totalPages = await Service.SearchInternalProjectsTotalPages("",
                                                                       new ProjectFilterParams()
                                                                       {
                                                                           AmountOnPage = 10,
                                                                           Highlighted = false,
                                                                           Page = null,
                                                                           SortBy = null,
                                                                           SortDirection = "asc",
                                                                       });
            Assert.DoesNotThrow(() => {
                RepositoryMock.Verify(repository => repository.SearchCountAsync("", false), Times.Once);
            });

            Assert.AreEqual((int) Math.Ceiling(projects.Count / 10.0), totalPages);
        }

        /// <summary>
        /// Test whether the repository method is called with amount set to 20 if null and no changes are applied to the object.
        /// </summary>
        /// <param name="projects">The projects which are used as data to test..</param>
        [Test]
        public async Task SearchInternalProjectsTotalPages_goodflow_default_amountOnPage(
            [ProjectDataSourceAttribute(10)] List<Project> projects)
        {
            RepositoryMock.Setup(repository =>
                                     repository.SearchCountAsync(It.IsAny<string>(), It.IsAny<bool?>()))
                          .Returns(Task.FromResult(projects.Count));

            int totalPages = await Service.SearchInternalProjectsTotalPages("",
                                                                            new ProjectFilterParams()
                                                                            {
                                                                                AmountOnPage = null,
                                                                                Highlighted = false,
                                                                                Page = null,
                                                                                SortBy = null,
                                                                                SortDirection = "asc",
                                                                            });
            Assert.DoesNotThrow(() =>
            {
                RepositoryMock.Verify(repository => repository.SearchCountAsync("", false),
                                      Times.Once);
            });

            Assert.AreEqual((int) Math.Ceiling(projects.Count / 20.0), totalPages);
        }
    }
}
