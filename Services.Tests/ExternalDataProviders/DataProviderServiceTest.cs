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


using FluentAssertions;
using Models;
using Moq;
using NUnit.Framework;
using Repositories.Tests.DataSources;
using Services.ExternalDataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Tests.ExternalDataProviders
{

    /// <summary>
    ///     The data provider service tests class. This class tests the logic in the data provider service.
    /// </summary>
    /// <seealso cref="DataProviderServiceTest" />
    [TestFixture]
    public class DataProviderServiceTest
    {

        private IDataProviderService service;

        private Mock<IDataProviderLoader> loaderMock;

        private Mock<IDataSourceAdaptee> dataSourceMock;

        private List<IDataSourceAdaptee> dataSources;

        /// <summary>
        ///     Initialize runs before every test
        ///     Mock the data provider loader
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            dataSourceMock = new Mock<IDataSourceAdaptee>();
            dataSourceMock.As<IAuthorizedDataSourceAdaptee>();
            dataSourceMock.As<IPublicDataSourceAdaptee>();
            dataSources = new List<IDataSourceAdaptee>();

            loaderMock = new Mock<IDataProviderLoader>();
            loaderMock.Setup(_ => _.GetDataSourceByGuid(It.IsAny<string>()))
                      .ReturnsAsync(dataSourceMock.Object);
            loaderMock.Setup(_ => _.GetDataSourceByName(It.IsAny<string>()))
                      .ReturnsAsync(dataSourceMock.Object);
            loaderMock.Setup(_ => _.GetAllDataSources())
                      .ReturnsAsync(dataSources);

            service = new DataProviderService(loaderMock.Object);
        }

        [Test]
        public async Task GetAllProjectsInAuthFlow_GoodFlow([ProjectDataSource(50)] IEnumerable<Project> projects)
        {
            // Arrange
            dataSourceMock.As<IAuthorizedDataSourceAdaptee>().Setup(_ => _.GetAllProjects(It.IsAny<string>()))
                          .ReturnsAsync(projects);

            // Act
            Action act = () => service.GetAllProjects(It.IsAny<string>(), It.IsAny<string>(), true);
            IEnumerable<Project> retrievedProjects =
                await service.GetAllProjects(It.IsAny<string>(), It.IsAny<string>(), true);

            // Assert
            act.Should().NotThrow();
            retrievedProjects.Should().BeEquivalentTo(projects);

        }

        [Test]
        public async Task GetAllProjectsInPublicFlow_GoodFlow([ProjectDataSource(50)] IEnumerable<Project> projects)
        {
            // Arrange
            dataSourceMock.As<IPublicDataSourceAdaptee>().Setup(_ => _.GetAllPublicProjects(It.IsAny<string>()))
                          .ReturnsAsync(projects);

            // Act
            Action act = () => service.GetAllProjects(It.IsAny<string>(), It.IsAny<string>(), false);
            IEnumerable<Project> retrievedProjects =
                await service.GetAllProjects(It.IsAny<string>(), It.IsAny<string>(), false);

            // Assert
            act.Should().NotThrow();
            retrievedProjects.Should().BeEquivalentTo(projects);

        }

        [Test]
        public async Task GetProjectByIdInAuthFlow_GoodFlow([ProjectDataSource] Project project)
        {
            // Arrange
            dataSourceMock.As<IAuthorizedDataSourceAdaptee>().Setup(_ => _.GetProjectById(It.IsAny<string>(), It.IsAny<string>()))
                          .ReturnsAsync(project);

            // Act
            Action act = () => service.GetProjectById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true);
            Project retrievedProject =
                await service.GetProjectById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true);

            // Assert
            act.Should().NotThrow();
            retrievedProject.Should().Be(project);
        }

        [Test]
        public async Task GetProjectByIdInAuthFlow_NoProjectsFound()
        {
            // Arrange
            dataSourceMock.As<IAuthorizedDataSourceAdaptee>()
                          .Setup(_ => _.GetProjectById(It.IsAny<string>(), It.IsAny<string>()))
                          .ReturnsAsync((Project) null);

            // Act
            Action act = () => service.GetProjectById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), false);
            Project retrievedProject =
                await service.GetProjectById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), false);

            // Assert
            act.Should().NotThrow();
            retrievedProject.Should().BeNull();
        }

        [Test]
        public async Task GetProjectByIdInPublicFlow_GoodFlow([ProjectDataSource] Project project)
        {
            // Arrange
            dataSourceMock.As<IPublicDataSourceAdaptee>().Setup(_ => _.GetPublicProjectById(It.IsAny<string>()))
                          .ReturnsAsync(project);

            // Act
            Action act = () => service.GetProjectById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), false);
            Project retrievedProject =
                await service.GetProjectById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), false);

            // Assert
            act.Should().NotThrow();
            retrievedProject.Should().Be(project);
        }

        [Test]
        public async Task GetProjectByIdInPublicFlow_NoProjectsFound()
        {
            // Arrange
            dataSourceMock.As<IPublicDataSourceAdaptee>()
                          .Setup(_ => _.GetPublicProjectById(It.IsAny<string>()))
                          .ReturnsAsync((Project) null);

            // Act
            Action act = () => service.GetProjectById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), false);
            Project retrievedProject =
                await service.GetProjectById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), false);

            // Assert
            act.Should().NotThrow();
            retrievedProject.Should().BeNull();
        }

        [Test]
        public async Task GetProjectFromUri_GoodFlow([ProjectDataSource] Project project)
        {
            // Arrange
            dataSourceMock.As<IPublicDataSourceAdaptee>()
                          .Setup(_ => _.GetPublicProjectFromUri(It.IsAny<Uri>()))
                          .ReturnsAsync(project);

            // Act
            Action act = () => service.GetProjectFromUri(It.IsAny<string>(), "https://google.nl/test");
            Project retrievedProject =
                await service.GetProjectFromUri(It.IsAny<string>(), "https://google.nl/test");

            // Assert
            act.Should().NotThrow();
            retrievedProject.Should().Be(project);
        }

        [Test]
        public async Task GetProjectFromUri_NoProjectFound()
        {
            // Arrange
            dataSourceMock.As<IPublicDataSourceAdaptee>()
                          .Setup(_ => _.GetPublicProjectFromUri(It.IsAny<Uri>()))
                          .ReturnsAsync((Project)null);

            // Act
            Action act = () => service.GetProjectFromUri(It.IsAny<string>(), "https://google.nl/test");
            Project retrievedProject =
                await service.GetProjectFromUri(It.IsAny<string>(), "https://google.nl/test");

            // Assert
            act.Should().NotThrow();
            retrievedProject.Should().BeNull();
        }

        [Test]
        public async Task RetrieveDataSourcesWithNoSearchSpecification_GoodFlow()
        {
            // Arrange
            dataSources.Add(new Mock<IPublicDataSourceAdaptee>().Object);
            dataSources.Add(new Mock<IAuthorizedDataSourceAdaptee>().Object);

            // Act
            Action act = () => service.RetrieveDataSources(null);
            IEnumerable<IDataSourceAdaptee> retrievedAdaptees = await service.RetrieveDataSources(null);

            // Assert
            act.Should().NotThrow();
            retrievedAdaptees.ToList().Count.Should().Be(2);

        }

        [Test]
        public async Task RetrieveDataSourcesWithNoSearchSpecification_NoAdapteesFound()
        {
            // Arrange

            // Act
            Action act = () => service.RetrieveDataSources(null);
            IEnumerable<IDataSourceAdaptee> retrievedAdaptees = await service.RetrieveDataSources(null);

            // Assert
            act.Should().NotThrow();
            retrievedAdaptees.ToList().Count.Should().Be(0);

        }

        [Test]
        public async Task RetrieveDataSourcesWithPublicSearchSpecification_GoodFlow()
        {
            // Arrange
            dataSources.Add(new Mock<IPublicDataSourceAdaptee>().Object);
            dataSources.Add(new Mock<IAuthorizedDataSourceAdaptee>().Object);

            // Act
            Action act = () => service.RetrieveDataSources(false);
            IEnumerable<IDataSourceAdaptee> retrievedAdaptees = await service.RetrieveDataSources(false);

            // Assert
            act.Should().NotThrow();
            retrievedAdaptees.ToList().Count.Should().Be(1);

        }

        [Test]
        public async Task RetrieveDataSourcesWithAutherizedSearchSpecification_GoodFlow()
        {
            // Arrange
            dataSources.Add(new Mock<IPublicDataSourceAdaptee>().Object);
            dataSources.Add(new Mock<IAuthorizedDataSourceAdaptee>().Object);

            // Act
            Action act = () => service.RetrieveDataSources(true);
            IEnumerable<IDataSourceAdaptee> retrievedAdaptees = await service.RetrieveDataSources(true);

            // Assert
            act.Should().NotThrow();
            retrievedAdaptees.ToList().Count.Should().Be(1);

        }

        [Test]
        public async Task RetrieveDataSourceByGuid_GoodFlow()
        {
            // Arrange

            // Act
            Action act = () => service.RetrieveDataSourceByGuid(It.IsAny<string>());
            IDataSourceAdaptee retrievedAdaptee = await service.RetrieveDataSourceByGuid(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedAdaptee.Should().NotBeNull();
            retrievedAdaptee.Should().BeAssignableTo(typeof(IDataSourceAdaptee));
        }

        [Test]
        public async Task RetrieveDataSourceByGuid_NoDataSourceFound()
        {
            // Arrange
            loaderMock.Setup(_ => _.GetDataSourceByGuid(It.IsAny<string>()))
                      .ReturnsAsync((IDataSourceAdaptee)null);

            // Act
            Action act = () => service.RetrieveDataSourceByGuid(It.IsAny<string>());
            IDataSourceAdaptee retrievedAdaptee = await service.RetrieveDataSourceByGuid(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedAdaptee.Should().BeNull();

        }

        [Test]
        public async Task RetrieveDataSourceByName_GoodFlow()
        {
            // Arrange

            // Act
            Action act = () => service.RetrieveDataSourceByName(It.IsAny<string>());
            IDataSourceAdaptee retrievedAdaptee = await service.RetrieveDataSourceByName(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedAdaptee.Should().NotBeNull();
            retrievedAdaptee.Should().BeAssignableTo(typeof(IDataSourceAdaptee));
        }

        [Test]
        public async Task RetrieveDataSourceByName_NoDataSourceFound()
        {
            // Arrange
            loaderMock.Setup(_ => _.GetDataSourceByName(It.IsAny<string>()))
                      .ReturnsAsync((IDataSourceAdaptee)null);

            // Act
            Action act = () => service.RetrieveDataSourceByName(It.IsAny<string>());
            IDataSourceAdaptee retrievedAdaptee = await service.RetrieveDataSourceByName(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedAdaptee.Should().BeNull();

        }

    }

}
