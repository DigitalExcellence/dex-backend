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
using Services.Tests.ExternalDataProviders.DataSources.Gitlab;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Tests.ExternalDataProviders
{

    [TestFixture]
    public class GitlabDataSourceAdapteeTest
    {

        /// <summary>
        ///     The IDataSourceAdaptee which in this case is the IGithubDataSourceAdaptee.
        /// </summary>
        private IGitlabDataSourceAdaptee dataSourceAdaptee;

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
                                                          {"App:DataSources:Gitlab", "valid_access_token"}
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
        public async Task FetchAllPublicGitlabRepositories_GoodFlow(
            [GitlabDataSourceResourceResultDataSource(50)]
            IEnumerable<GitlabDataSourceResourceResult> resourceResults)
        {
            // Arrange
            MockRestClient(resourceResults, HttpStatusCode.OK);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Action act = () => dataSourceAdaptee.FetchAllPublicGitlabRepositories(It.IsAny<string>());
            IEnumerable<GitlabDataSourceResourceResult> retrievedResourceResults =
                await dataSourceAdaptee.FetchAllPublicGitlabRepositories(It.IsAny<string>());

            // Assert
            act.Should()
               .NotThrow();
            retrievedResourceResults.Should()
                                    .BeEquivalentTo(resourceResults);

        }

        [Test]
        public async Task FetchAllPublicGitlabRepositories_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Func<Task> act = () => dataSourceAdaptee.FetchAllPublicGitlabRepositories(It.IsAny<string>());

            // Assert
            act.Should()
               .ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        [Test]
        public async Task FetchPublicRepository_GoodFlow(
            [GitlabDataSourceResourceResultDataSource]
            GitlabDataSourceResourceResult resourceResult)
        {
            // Arrange
            MockRestClient(resourceResult, HttpStatusCode.OK);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Uri testUri = new Uri("https://google.nl/test");
            Action act = () => dataSourceAdaptee.FetchPublicRepository(testUri);
            GitlabDataSourceResourceResult retrievedResourceResult =
                await dataSourceAdaptee.FetchPublicRepository(testUri);

            // Assert
            act.Should()
               .NotThrow();
            retrievedResourceResult.Should()
                                   .BeEquivalentTo(resourceResult);
        }

        [Test]
        public async Task FetchPublicRepository_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Uri testUri = new Uri("https://google.nl/test");
            Func<Task> act = () => dataSourceAdaptee.FetchPublicRepository(testUri);

            // Assert
            act.Should()
               .ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        [Test]
        public async Task FetchPublicGitlabRepositoryById_GoodFlow(
            [GitlabDataSourceResourceResultDataSource]
            GitlabDataSourceResourceResult resourceResult)
        {
            // Arrange
            MockRestClient(resourceResult, HttpStatusCode.OK);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Action act = () => dataSourceAdaptee.FetchPublicGitlabRepositoryById(It.IsAny<string>());
            GitlabDataSourceResourceResult retrievedResourceResult =
                await dataSourceAdaptee.FetchPublicGitlabRepositoryById(It.IsAny<string>());

            // Assert
            act.Should()
               .NotThrow();
            retrievedResourceResult.Should()
                                   .BeEquivalentTo(resourceResult);
        }

        [Test]
        public async Task FetchPublicGitlabRepositoryById_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Func<Task> act = () => dataSourceAdaptee.FetchPublicGitlabRepositoryById(It.IsAny<string>());

            // Assert
            act.Should()
               .ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        [Test]
        public async Task FetchAllGitlabRepositories_GoodFlow(
            [GitlabDataSourceResourceResultDataSource(50)]
            IEnumerable<GitlabDataSourceResourceResult> resourceResults)
        {
            // Arrange
            MockRestClient(resourceResults, HttpStatusCode.OK);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Action act = () => dataSourceAdaptee.FetchAllGitlabRepositories(It.IsAny<string>(), It.IsAny<string>());
            IEnumerable<GitlabDataSourceResourceResult> retrievedResourceResults =
                await dataSourceAdaptee.FetchAllGitlabRepositories(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            act.Should()
               .NotThrow();
            retrievedResourceResults.Should()
                                    .BeEquivalentTo(resourceResults);
        }

        [Test]
        public async Task FetchAllGitlabRepositories_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Func<Task> act = () => dataSourceAdaptee.FetchAllGitlabRepositories(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            act.Should()
               .ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        [Test]
        public async Task FetchGitlabRepositoryById_GoodFlow(
            [GitlabDataSourceResourceResultDataSource]
            GitlabDataSourceResourceResult resourceResult)
        {
            // Arrange
            MockRestClient(resourceResult, HttpStatusCode.OK);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Action act = () => dataSourceAdaptee.FetchGitlabRepositoryById(It.IsAny<string>(), It.IsAny<string>());
            GitlabDataSourceResourceResult retrievedResourceResult =
                await dataSourceAdaptee.FetchGitlabRepositoryById(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            act.Should()
               .NotThrow();
            retrievedResourceResult.Should()
                                   .BeEquivalentTo(resourceResult);
        }

        [Test]
        public async Task FetchGitlabRepositoryById_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Func<Task> act = () => dataSourceAdaptee.FetchGitlabRepositoryById(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            act.Should()
               .ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        [Test]
        public async Task FetchUserFromAccessToken_GoodFlow(
            [GitlabDataSourceUserResourceResultDataSource]
            GitlabDataSourceUserResourceResult userResourceResult)
        {
            // Arrange
            MockRestClient(userResourceResult, HttpStatusCode.OK);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Action act = () => dataSourceAdaptee.FetchUserFromAccessToken(It.IsAny<string>());
            GitlabDataSourceUserResourceResult retrievedUsers =
                await dataSourceAdaptee.FetchUserFromAccessToken(It.IsAny<string>());

            // Assert
            act.Should()
               .NotThrow();
            retrievedUsers.Should()
                          .BeEquivalentTo(userResourceResult);
        }

        [Test]
        public void FetchUserFromAccessToken_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Func<Task> act = () => dataSourceAdaptee.FetchUserFromAccessToken(It.IsAny<string>());

            // Assert
            act.Should()
               .ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);

        }

        [Test]
        public async Task FetchReadme_GoodFlow()
        {
            // Arrange
            string readmeContent = new string("This is the content from a test readme file");
            MockRestClient(readmeContent, HttpStatusCode.OK);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Action act = () => dataSourceAdaptee.FetchReadme("https://google.nl/test");
            string retrievedReadme = await dataSourceAdaptee.FetchReadme("https://google.nl/test");

            // Assert
            act.Should().NotThrow();
            retrievedReadme.Substring(1, retrievedReadme.Length-2).Should().Be(readmeContent);
        }

        [Test]
        public async Task FetchReadme_ContentNotFound()
        {
            // Arrange
            MockRestClient(null , HttpStatusCode.NotFound);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Action act = () => dataSourceAdaptee.FetchReadme("https://google.nl/test");
            string retrievedReadmeContent = await dataSourceAdaptee.FetchReadme("https://google.nl/test");

            // Assert
            act.Should().NotThrow();
            retrievedReadmeContent.Should().BeNullOrEmpty();

        }

        [Test]
        public async Task FetchReadme_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null , HttpStatusCode.BadRequest, errorMessage);
            dataSourceAdaptee = new GitlabDataSourceAdaptee(configurationMock, clientFactoryMock.Object, mapper);

            // Act
            Func<Task> act = () => dataSourceAdaptee.FetchReadme("https://google.nl/test");

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
