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
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using Services.ExternalDataProviders;
using Services.ExternalDataProviders.Resources;
using Services.Sources;
using Services.Tests.ExternalDataProviders.DataSources;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Tests.ExternalDataProviders
{

    [TestFixture]
    public class GithubDataSourceAdapteeTest
    {

        // TODO: Make base class for all the different data source adaptee tests.

        /// <summary>
        ///     The IDataSourceAdaptee which in this case is the IGithubDataSourceAdaptee.
        /// </summary>
        private IGithubDataSourceAdaptee dataSourceAdaptee;

        /// <summary>
        ///     A mock of the configuration.
        /// </summary>
        private IConfiguration configurationMock;

        /// <summary>
        ///     The actual mapper.
        /// </summary>
        private IMapper mapper;

        private Mock<IRestClient> clientMock;

        private Mock<IRestClientFactory> clientFactoryMock;

        /// <summary>
        ///     Initialize runs before every test
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            Dictionary<string, string> inMemoryConf = new Dictionary<string, string>
                                                      {
                                                          {"App:DataSources:Github", "valid_access_token"}
                                                      };

            configurationMock = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemoryConf)
                                .Build();

            clientMock = new Mock<IRestClient>();

            clientFactoryMock = new Mock<IRestClientFactory>();
            clientFactoryMock
                .Setup(_ => _.Create(It.IsAny<Uri>()))
                .Returns(clientMock.Object);

            MapperConfiguration mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
            mapper = mappingConfig.CreateMapper();
        }

        [Test]
        public async Task FetchAllGithubProjects_GoodFlow(
            [GithubDataSourceResourceResultDataSource(30)] IEnumerable<GithubDataSourceResourceResult> resourceResults)
        {
            // Arrange
            MockRestClient(resourceResults, HttpStatusCode.OK);
            dataSourceAdaptee = new GithubDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Action act = () => dataSourceAdaptee.FetchAllGithubProjects(It.IsAny<string>());
            IEnumerable<GithubDataSourceResourceResult> retrievedResourceResults = await dataSourceAdaptee.FetchAllGithubProjects(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedResourceResults.Should().BeEquivalentTo(resourceResults);

        }

        [Test]
        public void FetchAllGithubProjects_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(new GithubDataSourceResourceResult[0], HttpStatusCode.BadRequest, errorMessage);
            dataSourceAdaptee = new GithubDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Func<Task> act = () => dataSourceAdaptee.FetchAllGithubProjects(It.IsAny<string>());

            // Assert
            act.Should().ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        [Test]
        public async Task FetchAllPublicGithubRepositories_GoodFlow(
            [GithubDataSourceResourceResultDataSource(30)] IEnumerable<GithubDataSourceResourceResult> resourceResults)
        {
            // Arrange
            MockRestClient(resourceResults, HttpStatusCode.OK);
            dataSourceAdaptee = new GithubDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Action act = () => dataSourceAdaptee.FetchAllGithubProjects(It.IsAny<string>());
            IEnumerable<GithubDataSourceResourceResult> retrievedResourceResults = await dataSourceAdaptee.FetchAllPublicGithubRepositories(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedResourceResults.Should().BeEquivalentTo(resourceResults);
        }

        [Test]
        public void FetchAllPublicGithubRepositories_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(new GithubDataSourceResourceResult[0], HttpStatusCode.BadRequest, errorMessage);
            dataSourceAdaptee = new GithubDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Func<Task> act = () => dataSourceAdaptee.FetchAllPublicGithubRepositories(It.IsAny<string>());

            // Assert
            act.Should().ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        [Test]
        public async Task FetchPublicRepository_GoodFlow(
            [GithubDataSourceResourceResultDataSource] GithubDataSourceResourceResult resourceResult)
        {
            // Arrange
            MockRestClient(resourceResult, HttpStatusCode.OK);
            dataSourceAdaptee = new GithubDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Uri testUri = new Uri("https://google.nl/test");
            Action act = () => dataSourceAdaptee.FetchPublicRepository(testUri);
            GithubDataSourceResourceResult retrievedResourceResult = await dataSourceAdaptee.FetchPublicRepository(testUri);

            // Assert
            act.Should().NotThrow();
            retrievedResourceResult.Should().BeEquivalentTo(resourceResult);
        }

        [Test]
        public void FetchPublicRepository_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null , HttpStatusCode.BadRequest, errorMessage);
            dataSourceAdaptee = new GithubDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Uri testUri = new Uri("https://google.nl/test");
            Func<Task> act = () => dataSourceAdaptee.FetchPublicRepository(testUri);

            // Assert
            act.Should().ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        [Test]
        public async Task FetchPublicGithubProjectById_GoodFlow(
            [GithubDataSourceResourceResultDataSource] GithubDataSourceResourceResult resourceResult)
        {
            // Arrange
            MockRestClient(resourceResult, HttpStatusCode.OK);
            dataSourceAdaptee = new GithubDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Action act = () => dataSourceAdaptee.FetchPublicGithubProjectById(It.IsAny<string>());
            GithubDataSourceResourceResult retrievedResourceResult = await dataSourceAdaptee.FetchPublicGithubProjectById(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedResourceResult.Should().BeEquivalentTo(resourceResult);
        }

        [Test]
        public void FetchPublicGithubProjectById_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null , HttpStatusCode.BadRequest, errorMessage);
            dataSourceAdaptee = new GithubDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Func<Task> act = () => dataSourceAdaptee.FetchPublicGithubProjectById(It.IsAny<string>());

            // Assert
            act.Should().ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        [Test]
        public async Task FetchReadme_GoodFlow(
            [GithubDataSourceReadmeResourceResultDataSource] GithubDataSourceReadmeResourceResult resourceResult)
        {
            // Arrange
            MockRestClient(resourceResult, HttpStatusCode.OK);
            dataSourceAdaptee = new GithubDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Action act = () => dataSourceAdaptee.FetchReadme(It.IsAny<string>(),It.IsAny<string>());
            string retrievedResourceResult = await dataSourceAdaptee.FetchReadme(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedResourceResult.Should().BeEquivalentTo(JsonConvert.SerializeObject(resourceResult));
        }

        [Test]
        public async Task FetchReadme_ReadmeNotFound()
        {
            // Arrange
            MockRestClient(null , HttpStatusCode.NotFound);
            dataSourceAdaptee = new GithubDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Func<Task> act = () => dataSourceAdaptee.FetchReadme(It.IsAny<string>(), It.IsAny<string>());
            string retrievedReadme = await dataSourceAdaptee.FetchReadme(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedReadme.Should().BeNullOrEmpty();
        }

        [Test]
        public void FetchReadme_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null , HttpStatusCode.BadRequest, errorMessage);
            dataSourceAdaptee = new GithubDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Func<Task> act = () => dataSourceAdaptee.FetchReadme(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            act.Should().ThrowExactly<ExternalException>().WithMessage(errorMessage);
        }

        [Test]
        public async Task FetchReadmeContent_GoodFlow()
        {
            // Arrange
            string readmeContent = new string("This is the content from a test readme file");
            MockRestClient(readmeContent, HttpStatusCode.OK);
            dataSourceAdaptee = new GithubDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Action act = () => dataSourceAdaptee.FetchReadmeContent(It.IsAny<Uri>());
            string retrievedResourceResult = await dataSourceAdaptee.FetchReadmeContent(It.IsAny<Uri>());

            // Assert
            act.Should().NotThrow();
            retrievedResourceResult.Substring(1, retrievedResourceResult.Length-2).Should().BeEquivalentTo(readmeContent);
        }

        [Test]
        public async Task FetchReadmeContent_ContentNotFound()
        {
            // Arrange
            MockRestClient(null , HttpStatusCode.NotFound);
            dataSourceAdaptee = new GithubDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Func<Task> act = () => dataSourceAdaptee.FetchReadmeContent(It.IsAny<Uri>());
            string retrievedReadmeContent = await dataSourceAdaptee.FetchReadmeContent(It.IsAny<Uri>());

            // Assert
            act.Should().NotThrow();
            retrievedReadmeContent.Should().BeNullOrEmpty();
        }

        [Test]
        public void FetchReadmeContent_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null , HttpStatusCode.BadRequest, errorMessage);
            dataSourceAdaptee = new GithubDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Func<Task> act = () => dataSourceAdaptee.FetchReadmeContent(It.IsAny<Uri>());

            // Assert
            act.Should().ThrowExactly<ExternalException>().WithMessage(errorMessage);
        }

        private void MockRestClient(object result, HttpStatusCode statusCode, string errorMessage = null)
        {
            Mock<IRestResponse> response = new Mock<IRestResponse>();
            response.Setup(_ => _.Content)
                    .Returns(JsonConvert.SerializeObject(result));

            response.Setup(_ => _.StatusCode)
                    .Returns(statusCode);

            response.Setup(_ => _.IsSuccessful)
                    .Returns(() => (int) statusCode >= 200 && (int) statusCode <= 299);

            response.Setup(_ => _.ErrorMessage)
                    .Returns(() => errorMessage);

            clientMock.Setup(_ => _.ExecuteAsync(It.IsAny<IRestRequest>(), CancellationToken.None))
                      .ReturnsAsync(response.Object);
        }


    }

}
