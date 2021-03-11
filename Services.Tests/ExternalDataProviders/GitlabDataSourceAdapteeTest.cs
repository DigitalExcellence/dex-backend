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
using Microsoft.Extensions.Configuration;
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

    [TestFixture]
    public class GitlabDataSourceAdapteeTest : AdapteeTest<IGitlabDataSourceAdaptee>
    {

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

            ConfigurationMock = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemoryConf)
                                .Build();
        }

        [Test]
        public async Task FetchAllPublicGitlabRepositories_GoodFlow(
            [GitlabDataSourceResourceResultDataSource(50)]
            IEnumerable<GitlabDataSourceResourceResult> resourceResults)
        {
            // Arrange
            MockRestClient(resourceResults, HttpStatusCode.OK);
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

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

        [Test]
        public void FetchAllPublicGitlabRepositories_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchAllPublicGitlabRepositories(It.IsAny<string>());

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
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

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

        [Test]
        public void FetchPublicRepository_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Uri testUri = new Uri("https://google.nl/test");
            Func<Task> act = () => DataSourceAdaptee.FetchPublicRepository(testUri);

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
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

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

        [Test]
        public void FetchPublicGitlabRepositoryById_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchPublicGitlabRepositoryById(It.IsAny<string>());

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
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

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

        [Test]
        public void FetchAllGitlabRepositories_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchAllGitlabRepositories(It.IsAny<string>(), It.IsAny<string>());

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
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

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

        [Test]
        public void FetchGitlabRepositoryById_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchGitlabRepositoryById(It.IsAny<string>(), It.IsAny<string>());

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
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

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

        [Test]
        public void FetchUserFromAccessToken_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null, HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchUserFromAccessToken(It.IsAny<string>());

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
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchReadme("https://google.nl/test");
            string retrievedReadme = await DataSourceAdaptee.FetchReadme("https://google.nl/test");

            // Assert
            act.Should().NotThrow();
            retrievedReadme.Substring(1, retrievedReadme.Length-2).Should().Be(readmeContent);
        }

        [Test]
        public async Task FetchReadme_ContentNotFound()
        {
            // Arrange
            MockRestClient(null , HttpStatusCode.NotFound);
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchReadme("https://google.nl/test");
            string retrievedReadmeContent = await DataSourceAdaptee.FetchReadme("https://google.nl/test");

            // Assert
            act.Should().NotThrow();
            retrievedReadmeContent.Should().BeNullOrEmpty();

        }

        [Test]
        public void FetchReadme_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null , HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = new GitlabDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchReadme("https://google.nl/test");

            // Assert
            act.Should().ThrowExactly<ExternalException>().WithMessage(errorMessage);
        }

    }

}
