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

    [TestFixture]
    public class DataLoaderTest
    {

        private IDataProviderLoader loader;

        private Mock<IServiceScopeFactory> factoryMock;
        private Mock<IDataSourceModelRepository> repositoryMock;
        private Mock<IAssemblyHelper> assemblyHelperMock;
        private IMapper mapper;

        private Type[] returnedTypesFromAssembly;

        [SetUp]
        public void Initialize()
        {
            Mock<IServiceScope> scopeMock = new Mock<IServiceScope>();
            scopeMock
                .Setup(_ => _.ServiceProvider.GetService(It.IsAny<Type>()))
                .Returns(new object());

            factoryMock = new Mock<IServiceScopeFactory>();
            factoryMock.Setup(_ => _.CreateScope())
                       .Returns(scopeMock.Object);

            repositoryMock = new Mock<IDataSourceModelRepository>();

            assemblyHelperMock = new Mock<IAssemblyHelper>();

            MapperConfiguration mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
            mapper = mappingConfig.CreateMapper();

            loader = new DataProviderLoader(factoryMock.Object, repositoryMock.Object, assemblyHelperMock.Object ,mapper);
        }


        [Test]
        public async Task GetAllDataSources_GoodFlow()
        {
            // Arrange
            returnedTypesFromAssembly = new[]
                                        {
                                            typeof(GithubDataSourceAdaptee),
                                            typeof(GitlabDataSourceAdaptee),
                                            typeof(JsFiddleDataSourceAdaptee),
                                        };

            assemblyHelperMock.Setup(_ => _.RetrieveTypesFromExecutingAssemblyFolderFiles())
                              .Returns(returnedTypesFromAssembly);

            // Act
            Action act = () => loader.GetAllDataSources();
            List<IDataSourceAdaptee> retrievedDataSources = await loader.GetAllDataSources();

            // Assert
            act.Should().NotThrow();
            retrievedDataSources.Count.Should().Be(3);
            repositoryMock.Verify(_ => _.AddRangeAsync(It.IsAny<IEnumerable<DataSource>>()), Times.Exactly(2));
            repositoryMock.Verify(_ => _.RemoveAsync(It.IsAny<int>()), Times.Never);

        }

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


    }

}
