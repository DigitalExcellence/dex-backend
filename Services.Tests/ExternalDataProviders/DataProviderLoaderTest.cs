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

using API.Configuration;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Moq;
using NUnit.Framework;
using Repositories;
using Services.ExternalDataProviders;
using Services.ExternalDataProviders.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Tests.ExternalDataProviders
{

    /// <summary>
    ///     DataProviderLoaderTest
    /// </summary>
    /// <seealso cref="DataProviderLoader" />
    [TestFixture]
    public class DataProviderLoaderTest
    {

        private IDataProviderLoader loader;

        private Mock<IServiceScopeFactory> factoryMock;
        private Mock<IDataSourceModelRepository> repositoryMock;
        private Mock<IAssemblyHelper> assemblyHelperMock;
        private IMapper mapper;

        private Mock<IDataSourceAdaptee> returnedTypeFromLoader;

        private Type[] returnedTypesFromAssembly;

        [SetUp]
        public void Initialize()
        {
            factoryMock = new Mock<IServiceScopeFactory>();

            repositoryMock = new Mock<IDataSourceModelRepository>();

            assemblyHelperMock = new Mock<IAssemblyHelper>();

            MapperConfiguration mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
            mapper = mappingConfig.CreateMapper();

            loader = new DataProviderLoader(factoryMock.Object, repositoryMock.Object, assemblyHelperMock.Object ,mapper);
        }

        /// <summary>
        ///     This method tests the GetAllDataSources method in a good flow. In this scenario data sources do
        ///     exist and are found.
        /// </summary>
        /// <returns>The tested method will return the correct collection of data sources.</returns>
        [Test]
        public async Task GetAllDataSources_GoodFlow()
        {
            // Arrange
            returnedTypesFromAssembly = new[]
                                        {
                                            typeof(GithubDataSourceAdaptee)
                                        };

            assemblyHelperMock.Setup(_ => _.RetrieveTypesFromExecutingAssemblyFolderFolderByInterface(It.IsAny<Type>()))
                              .Returns(returnedTypesFromAssembly);

            returnedTypeFromLoader = new Mock<IDataSourceAdaptee>().As<IDataSourceAdaptee>();

            Mock<IServiceScope> scopeMock = new Mock<IServiceScope>();
            scopeMock
                .Setup(_ => _.ServiceProvider.GetService(It.IsAny<Type>()))
                .Returns(returnedTypeFromLoader.Object);

            factoryMock.Setup(_ => _.CreateScope())
                       .Returns(scopeMock.Object);

            // Act
            Action act = () => loader.GetAllDataSources();
            List<IDataSourceAdaptee> retrievedDataSources = await loader.GetAllDataSources();

            // Assert
            act.Should().NotThrow();
            retrievedDataSources.Count.Should().Be(1);
            repositoryMock.Verify(_ => _.AddRangeAsync(It.IsAny<IEnumerable<DataSource>>()), Times.Exactly(2));
            repositoryMock.Verify(_ => _.RemoveAsync(It.IsAny<int>()), Times.Never);

        }

        /// <summary>
        ///     This method tests the GetAllDataSources method in a flow where no data sources
        ///     are found.
        /// </summary>
        /// <returns>The tested method will return an empty collection of data sources.</returns>
        [Test]
        public async Task GetAllDataSources_NoDataSources()
        {
            // Arrange
            returnedTypesFromAssembly = new Type[0];

            assemblyHelperMock.Setup(_ => _.RetrieveTypesFromExecutingAssemblyFolderFiles())
                              .Returns(returnedTypesFromAssembly);

            // Act
            Action act = () => loader.GetAllDataSources();
            List<IDataSourceAdaptee> retrievedDataSources = await loader.GetAllDataSources();

            // Assert
            act.Should().NotThrow();
            retrievedDataSources.Count.Should().Be(0);
            repositoryMock.Verify(_ => _.AddRangeAsync(It.IsAny<IEnumerable<DataSource>>()), Times.Never);
            repositoryMock.Verify(_ => _.RemoveAsync(It.IsAny<int>()), Times.Never);

        }

        /// <summary>
        ///     This method tests the GetAllDataSources method in a flow where no local data sources are found,
        ///     however there are implementations found in the data base.
        /// </summary>
        /// <returns>
        ///     The tested method will return an empty collection and the implementations will get removed from the database.
        /// </returns>
        [Test]
        public async Task GetAllDataSources_NoDataSourcesLocalButDataSourceInDb()
        {
            // Arrange
            returnedTypesFromAssembly = new Type[0];
            repositoryMock.Setup(_ => _.GetAll())
                          .ReturnsAsync(new List<DataSource>
                                        {
                                            new Mock<DataSource>().Object
                                        });

            assemblyHelperMock.Setup(_ => _.RetrieveTypesFromExecutingAssemblyFolderFiles())
                              .Returns(returnedTypesFromAssembly);

            // Act
            Action act = () => loader.GetAllDataSources();
            List<IDataSourceAdaptee> retrievedDataSources = await loader.GetAllDataSources();

            // Assert
            act.Should().NotThrow();
            retrievedDataSources.Count.Should().Be(0);
            repositoryMock.Verify(_ => _.AddRangeAsync(It.IsAny<IEnumerable<DataSource>>()), Times.Never);
            repositoryMock.Verify(_ => _.RemoveAsync(It.IsAny<int>()), Times.Exactly(2));

        }

        /// <summary>
        ///     This method tests the GetDataSourceById method in a good flow. In this scenario a data source with the
        ///     specified guid exists and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct data source with the specified guid.</returns>
        [Test]
        public async Task GetDataSourceByGuid_GoodFlow()
        {
            // Arrange
            returnedTypesFromAssembly = new[]
                                        {
                                            typeof(GithubDataSourceAdaptee)
                                        };

            assemblyHelperMock.Setup(_ => _.RetrieveTypesFromExecutingAssemblyFolderFolderByInterface(It.IsAny<Type>()))
                              .Returns(returnedTypesFromAssembly);

            string guid = "de38e528-1d6d-40e7-83b9-4334c51c19be";
            returnedTypeFromLoader = new Mock<IDataSourceAdaptee>().As<IDataSourceAdaptee>();
            returnedTypeFromLoader.Setup(_ => _.Guid)
                                  .Returns(guid);

            Mock<IServiceScope> scopeMock = new Mock<IServiceScope>();
            scopeMock
                .Setup(_ => _.ServiceProvider.GetService(It.IsAny<Type>()))
                .Returns(returnedTypeFromLoader.Object);

            factoryMock.Setup(_ => _.CreateScope())
                       .Returns(scopeMock.Object);

            // Act
            Action act = () => loader.GetDataSourceByGuid(guid);
            IDataSourceAdaptee retrievedDataSource = await loader.GetDataSourceByGuid(guid);

            // Assert
            act.Should().NotThrow();
            retrievedDataSource.Should().NotBeNull();
            retrievedDataSource.Guid.Should().Be(guid);
            scopeMock.Verify(_ => _.ServiceProvider.GetService(It.IsAny<Type>()), Times.Exactly(2));

        }

        /// <summary>
        ///     This method tests the GetDataSourceById method in a flow where no data source could be found
        ///     with the specified guid.
        /// </summary>
        /// <returns>The tested method will return no data source.</returns>
        [Test]
        public async Task GetDataSourceByGuid_NoDataSourceFound()
        {
            // Arrange
            assemblyHelperMock.Setup(_ => _.RetrieveTypesFromExecutingAssemblyFolderFolderByInterface(It.IsAny<Type>()))
                              .Returns(new Type[0]);

            Mock<IServiceScope> scopeMock = new Mock<IServiceScope>();
            scopeMock
                .Setup(_ => _.ServiceProvider.GetService(It.IsAny<Type>()))
                .Returns((IDataSourceAdaptee)null);

            factoryMock.Setup(_ => _.CreateScope())
                       .Returns(scopeMock.Object);

            // Act
            Action act = () => loader.GetDataSourceByGuid(It.IsAny<string>());
            IDataSourceAdaptee retrievedDataSource = await loader.GetDataSourceByGuid(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedDataSource.Should().BeNull();
            scopeMock.Verify(_ => _.ServiceProvider.GetService(It.IsAny<Type>()), Times.Exactly(0));

        }

        /// <summary>
        ///     This method tests the GetDataSourceByName method in a good flow. In this scenario a data source with the
        ///     specified name exists and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct data source with the specified guid.</returns>
        [Test]
        public async Task GetDataSourceByName_GoodFlow()
        {
            // Arrange
            returnedTypesFromAssembly = new[]
                                        {
                                            typeof(GithubDataSourceAdaptee)
                                        };

            assemblyHelperMock.Setup(_ => _.RetrieveTypesFromExecutingAssemblyFolderFolderByInterface(It.IsAny<Type>()))
                              .Returns(returnedTypesFromAssembly);

            string name = "TestSource";
            returnedTypeFromLoader = new Mock<IDataSourceAdaptee>().As<IDataSourceAdaptee>();
            returnedTypeFromLoader.Setup(_ => _.Title)
                                  .Returns(name);

            Mock<IServiceScope> scopeMock = new Mock<IServiceScope>();
            scopeMock
                .Setup(_ => _.ServiceProvider.GetService(It.IsAny<Type>()))
                .Returns(returnedTypeFromLoader.Object);

            factoryMock.Setup(_ => _.CreateScope())
                       .Returns(scopeMock.Object);

            // Act
            Action act = () => loader.GetDataSourceByName(name);
            IDataSourceAdaptee retrievedDataSource = await loader.GetDataSourceByName(name);

            // Assert
            act.Should().NotThrow();
            retrievedDataSource.Should().NotBeNull();
            retrievedDataSource.Title.Should().Be(name);
            scopeMock.Verify(_ => _.ServiceProvider.GetService(It.IsAny<Type>()), Times.Exactly(2));

        }

        /// <summary>
        ///     This method tests the GetDataSourceByName method in a flow where no data source could be found
        ///     with the specified name.
        /// </summary>
        /// <returns>The tested method will return no data source.</returns>
        [Test]
        public async Task GetDataSourceByName_NoDataSourceFound()
        {
            // Arrange
            assemblyHelperMock.Setup(_ => _.RetrieveTypesFromExecutingAssemblyFolderFolderByInterface(It.IsAny<Type>()))
                              .Returns(new Type[0]);

            Mock<IServiceScope> scopeMock = new Mock<IServiceScope>();
            scopeMock
                .Setup(_ => _.ServiceProvider.GetService(It.IsAny<Type>()))
                .Returns((IDataSourceAdaptee)null);

            factoryMock.Setup(_ => _.CreateScope())
                       .Returns(scopeMock.Object);

            // Act
            Action act = () => loader.GetDataSourceByName(It.IsAny<string>());
            IDataSourceAdaptee retrievedDataSource = await loader.GetDataSourceByName(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedDataSource.Should().BeNull();
            scopeMock.Verify(_ => _.ServiceProvider.GetService(It.IsAny<Type>()), Times.Exactly(0));

        }


    }

}
