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

using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Models;
using Moq;
using NUnit.Framework;
using Services.ExternalDataProviders;
using Services.ExternalDataProviders.Resources;
using Services.Tests.Base;
using Services.Tests.ExternalDataProviders.DataSources.Gitlab;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Services.Tests.ExternalDataProviders
{

    /// <summary>
    ///     GitlabDataSourceAdapteeTest
    /// </summary>
    /// <seealso cref="IGitlabDataSourceAdaptee" />
    [TestFixture]
    public class GitlabDataSourceAdapteeTest : AdapteeTest<IGitlabDataSourceAdaptee>
    {
        /// <summary>
        /// Defines the way a gitlab datasource adaptee is created, is by default <see cref="GitlabDataSourceAdaptee"/>.
        /// </summary>
        protected Func<IConfiguration, Sources.IRestClientFactory, IMapper, GitlabDataSourceAdaptee> CreateDataSourceAdaptee;

        public GitlabDataSourceAdapteeTest()
        {
            CreateDataSourceAdaptee = (configuration, restClientFactory, mapper)
                => new GitlabDataSourceAdaptee(configuration, restClientFactory, mapper);
        }

        /// <summary>
        ///     Initialize runs before every test
        /// </summary>
        [SetUp]
        public virtual void Initialize()
        {
            Dictionary<string, string> inMemoryConf = new Dictionary<string, string>
                                                      {
                                                          {"App:DataSources:Gitlab:OauthUrl", "valid_oauth_url"}
                                                      };

            ConfigurationMock = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemoryConf)
                                .Build();
        }

        /// <summary>
        ///     This method tests the FetchAllPublicGitlabRepositories method in a good flow. In this scenario
        ///     gitlab data sources exist and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct collection of resource results.</returns>
        [Test]
        public async Task FetchAllPublicGitlabRepositories_GoodFlow(
            [GitlabDataSourceResourceResultDataSource(50)]
            IEnumerable<GitlabDataSourceResourceResult> resourceResults)
        {
            // Arrange
            MockRestClient(resourceResults, HttpStatusCode.OK);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchAllPublicGitlabRepositories(It.IsAny<string>());
            IEnumerable<GitlabDataSourceResourceResult> retrievedResourceResults =
                await DataSourceAdaptee.FetchAllPublicGitlabRepositories(It.IsAny<string>());

            // Assert
            act.Should()
               .NotThrow();
            retrievedResourceResults.Should()
                                    .BeEquivalentTo(resourceResults);

        }

        /// <summary>
        ///     This method tests the FetchAllPublicGitlabRepositories method in a bad flow where the http status code
        ///     from the response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public void FetchAllPublicGitlabRepositories_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchAllPublicGitlabRepositories(It.IsAny<string>());

            // Assert
            act.Should()
               .ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        /// <summary>
        ///     This method tests the FetchPublicRepository method in a good flow. In this scenario
        ///     the gitlab data source exists and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct resource result.</returns>
        [Test]
        public async Task FetchPublicRepository_GoodFlow(
            [GitlabDataSourceResourceResultDataSource]
            GitlabDataSourceResourceResult resourceResult)
        {
            // Arrange
            MockRestClient(resourceResult, HttpStatusCode.OK);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Uri testUri = new Uri("https://google.nl/test");
            Action act = () => DataSourceAdaptee.FetchPublicRepository(testUri);
            GitlabDataSourceResourceResult retrievedResourceResult =
                await DataSourceAdaptee.FetchPublicRepository(testUri);

            // Assert
            act.Should()
               .NotThrow();
            retrievedResourceResult.Should()
                                   .BeEquivalentTo(resourceResult);
        }

        /// <summary>
        ///     This method tests the FetchPublicRepository method in a bad flow where the http status code
        ///     from the response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public void FetchPublicRepository_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Uri testUri = new Uri("https://google.nl/test");
            Func<Task> act = () => DataSourceAdaptee.FetchPublicRepository(testUri);

            // Assert
            act.Should()
               .ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        /// <summary>
        ///     This method tests the FetchPublicGitlabRepositoryById method in a good flow. In this scenario
        ///     the gitlab data source with the specified identifier exists and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct resource result with the specified identifier.</returns>
        [Test]
        public async Task FetchPublicGitlabRepositoryById_GoodFlow(
            [GitlabDataSourceResourceResultDataSource]
            GitlabDataSourceResourceResult resourceResult)
        {
            // Arrange
            MockRestClient(resourceResult, HttpStatusCode.OK);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchPublicGitlabRepositoryById(It.IsAny<string>());
            GitlabDataSourceResourceResult retrievedResourceResult =
                await DataSourceAdaptee.FetchPublicGitlabRepositoryById(It.IsAny<string>());

            // Assert
            act.Should()
               .NotThrow();
            retrievedResourceResult.Should()
                                   .BeEquivalentTo(resourceResult);
        }

        /// <summary>
        ///     This method tests the FetchPublicGitlabRepositoryById method in a bad flow where the http status code
        ///     from the response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public void FetchPublicGitlabRepositoryById_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchPublicGitlabRepositoryById(It.IsAny<string>());

            // Assert
            act.Should()
               .ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        /// <summary>
        ///     This method tests the FetchAllGitlabRepositories method in a good flow. In this scenario
        ///     the gitlab data sources exist and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct resource results.</returns>
        [Test]
        public async Task FetchAllGitlabRepositories_GoodFlow(
            [GitlabDataSourceResourceResultDataSource(50)]
            IEnumerable<GitlabDataSourceResourceResult> resourceResults)
        {
            // Arrange
            MockRestClient(resourceResults, HttpStatusCode.OK);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchAllGitlabRepositories(It.IsAny<string>(), It.IsAny<string>());
            IEnumerable<GitlabDataSourceResourceResult> retrievedResourceResults =
                await DataSourceAdaptee.FetchAllGitlabRepositories(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            act.Should()
               .NotThrow();
            retrievedResourceResults.Should()
                                    .BeEquivalentTo(resourceResults);
        }

        /// <summary>
        ///     This method tests the FetchAllGitlabRepositories method in a bad flow where the http status code
        ///     from the response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public void FetchAllGitlabRepositories_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchAllGitlabRepositories(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            act.Should()
               .ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        /// <summary>
        ///     This method tests the FetchGitlabRepositoryById method in a good flow. In this scenario
        ///     the gitlab data sources with the specified identifier is found and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct resource result.</returns>
        [Test]
        public async Task FetchGitlabRepositoryById_GoodFlow(
            [GitlabDataSourceResourceResultDataSource]
            GitlabDataSourceResourceResult resourceResult)
        {
            // Arrange
            MockRestClient(resourceResult, HttpStatusCode.OK);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchGitlabRepositoryById(It.IsAny<string>(), It.IsAny<string>());
            GitlabDataSourceResourceResult retrievedResourceResult =
                await DataSourceAdaptee.FetchGitlabRepositoryById(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            act.Should()
               .NotThrow();
            retrievedResourceResult.Should()
                                   .BeEquivalentTo(resourceResult);
        }

        /// <summary>
        ///     This method tests the FetchGitlabRepositoryById method in a bad flow where the http status code
        ///     from the response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public void FetchGitlabRepositoryById_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchGitlabRepositoryById(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            act.Should()
               .ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        /// <summary>
        ///     This method tests the FetchUserFromAccessToken method in a good flow. In this scenario
        ///     the gitlab user data sources from the access token is found and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct user resource result.</returns>
        [Test]
        public async Task FetchUserFromAccessToken_GoodFlow(
            [GitlabDataSourceUserResourceResultDataSource]
            GitlabDataSourceUserResourceResult userResourceResult)
        {
            // Arrange
            MockRestClient(userResourceResult, HttpStatusCode.OK);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchUserFromAccessToken(It.IsAny<string>());
            GitlabDataSourceUserResourceResult retrievedUsers =
                await DataSourceAdaptee.FetchUserFromAccessToken(It.IsAny<string>());

            // Assert
            act.Should()
               .NotThrow();
            retrievedUsers.Should()
                          .BeEquivalentTo(userResourceResult);
        }

        /// <summary>
        ///     This method tests the FetchUserFromAccessToken method in a bad flow where the http status code
        ///     from the response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public void FetchUserFromAccessToken_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchUserFromAccessToken(It.IsAny<string>());

            // Assert
            act.Should()
               .ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);

        }

        /// <summary>
        ///     This method tests the FetchReadme method in a good flow. In this scenario
        ///     the readme content is found and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct readme content.</returns>
        [Test]
        public virtual async Task FetchReadme_GoodFlow()
        {
            // Arrange
            string readmeContent = new string("This is the content from a test readme file");
            MockRestClient(readmeContent, HttpStatusCode.OK);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchReadme("https://google.nl/test");
            string retrievedReadme = await DataSourceAdaptee.FetchReadme("https://google.nl/test");

            // Assert
            act.Should().NotThrow();
            retrievedReadme.Substring(1, retrievedReadme.Length - 2).Should().Be(readmeContent);
        }

        /// <summary>
        ///     This method tests the FetchReadme method in a flow where the readme content
        ///     is not found.
        /// </summary>
        /// <returns>The tested method will receive 404 Not Found from the external API.</returns>
        [Test]
        public virtual async Task FetchReadme_ContentNotFound()
        {
            // Arrange
            MockRestClient(null, HttpStatusCode.NotFound);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchReadme("https://google.nl/test");
            string retrievedReadmeContent = await DataSourceAdaptee.FetchReadme("https://google.nl/test");

            // Assert
            act.Should().NotThrow();
            retrievedReadmeContent.Should().BeNullOrEmpty();

        }

        /// <summary>
        ///     This method tests the FetchReadme method in a bad flow where the http status code
        ///     from the response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public virtual void FetchReadme_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchReadme("https://google.nl/test");

            // Assert
            act.Should().ThrowExactly<ExternalException>().WithMessage(errorMessage);
        }

        /// <summary>
        ///     This method tests the FetchOauthTokens method in a good flow. In this scenario
        ///     a Oauth tokens object will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct readme content.</returns>
        [Test]
        public async Task FetchOauthTokens_GoodFlow()
        {
            OauthTokens oauthTokens = new OauthTokens { AccessToken = "token" };
            MockRestClient(oauthTokens, HttpStatusCode.OK);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchOauthTokens(It.IsAny<string>());
            OauthTokens retrievedOauthTokens = await DataSourceAdaptee.FetchOauthTokens(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedOauthTokens.Should().BeEquivalentTo(oauthTokens);
            retrievedOauthTokens.Should().NotBeNull();

        }

        /// <summary>
        ///     This method tests the FetchOauthTokens method in a bad flow where the http status
        ///     code from the response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public void FetchOauthTokens_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchOauthTokens(It.IsAny<string>());

            // Assert
            act.Should().ThrowExactly<ExternalException>().WithMessage(errorMessage);
        }

        /// <summary>
        ///     This method tests the FetchContributorsFromRepository method in a good flow. In this scenario
        ///     a collection of contributor resource results will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct collection of contributor resource results.</returns>
        [Test]
        public async Task FetchContributorsFromRepository_GoodFlow(
            [GitLabDataSourceContributorResourceResultDataSource(30)] List<GitLabDataSourceContributorResourceResult> resourceResults)
        {
            MockRestClient(resourceResults, HttpStatusCode.OK);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchContributorsFromRepository(It.IsAny<int>());
            List<GitLabDataSourceContributorResourceResult> retrievedOauthTokens = await DataSourceAdaptee.FetchContributorsFromRepository(It.IsAny<int>());

            // Assert
            act.Should().NotThrow();
            retrievedOauthTokens.Should().BeEquivalentTo(resourceResults);
            retrievedOauthTokens.Should().NotBeNull();

        }

        /// <summary>
        ///     This method tests the FetchContributorsFromRepository method in a bad flow where the http status
        ///     code from the response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public void FetchContributorsFromRepository_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchContributorsFromRepository(It.IsAny<int>());

            // Assert
            act.Should().ThrowExactly<ExternalException>().WithMessage(errorMessage);
        }

        /// <summary>
        ///     Tests if the GitlabDataSourceAdaptee can be created without an oauthurl
        /// </summary>
        /// <returns>A NullReferenceExceptions</returns>
        [Test]
        public void CreateGitlabAdapteeWithoutOauthUrl_ThrowsNullReferenceException()
        {
            // Arrange
            IConfiguration configurationWithoutOauthUrl = new ConfigurationBuilder()
                                .Build();
            Func<GitlabDataSourceAdaptee> act = () => CreateDataSourceAdaptee(configurationWithoutOauthUrl, ClientFactoryMock.Object, Mapper);

            // Assert
            act.Should().ThrowExactly<NullReferenceException>();
        }

        /// <summary>
        ///     Tests if the GitlabDataSourceAdaptee can be created with a valid oauthurl
        /// </summary>
        /// <returns>A valid GitlabDataSourceAdaptee</returns>
        [Test]
        public void CreateGitlabAdapteeWithOauthUrl_ThrowNoExceptions()
        {
            // Arrange
            IConfiguration configurationWithoutOauthUrl = new ConfigurationBuilder()
                                .Build();
            Func<GitlabDataSourceAdaptee> act = () => CreateDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Assert
            act.Should().NotThrow<NullReferenceException>();
        }

    }

}
