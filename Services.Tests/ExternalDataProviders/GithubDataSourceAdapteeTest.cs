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
using Newtonsoft.Json;
using NUnit.Framework;
using Services.ExternalDataProviders;
using Services.ExternalDataProviders.Resources;
using Services.Tests.Base;
using Services.Tests.ExternalDataProviders.DataSources.Github;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Services.Tests.ExternalDataProviders
{

    [TestFixture]
    public class GithubDataSourceAdapteeTest : AdapteeTest<IGithubDataSourceAdaptee>
    {

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

            ConfigurationMock = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemoryConf)
                                .Build();
        }

        /// <summary>
        ///     This method tests the FetchAllGithubProjects method in a good flow. In this scenario github data sources
        ///     exist and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct collection of resource results.</returns>
        [Test]
        public async Task FetchAllGithubProjects_GoodFlow(
            [GithubDataSourceResourceResultDataSource(30)] IEnumerable<GithubDataSourceResourceResult> resourceResults)
        {
            // Arrange
            MockRestClient(resourceResults, HttpStatusCode.OK);
            DataSourceAdaptee = new GithubDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchAllGithubProjects(It.IsAny<string>());
            IEnumerable<GithubDataSourceResourceResult> retrievedResourceResults = await DataSourceAdaptee.FetchAllGithubProjects(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedResourceResults.Should().BeEquivalentTo(resourceResults);

        }

        /// <summary>
        ///     This method tests the FetchAllGithubProjects method in a bad flow where the http status code from the
        ///     response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public void FetchAllGithubProjects_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(new GithubDataSourceResourceResult[0], HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = new GithubDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchAllGithubProjects(It.IsAny<string>());

            // Assert
            act.Should().ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        /// <summary>
        ///     This method tests the FetchAllPublicGithubProjects method in a good flow. In this scenario github data sources
        ///     exist and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct collection of resource results.</returns>
        [Test]
        public async Task FetchAllPublicGithubRepositories_GoodFlow(
            [GithubDataSourceResourceResultDataSource(30)] IEnumerable<GithubDataSourceResourceResult> resourceResults)
        {
            // Arrange
            MockRestClient(resourceResults, HttpStatusCode.OK);
            DataSourceAdaptee = new GithubDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchAllGithubProjects(It.IsAny<string>());
            IEnumerable<GithubDataSourceResourceResult> retrievedResourceResults = await DataSourceAdaptee.FetchAllPublicGithubRepositories(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedResourceResults.Should().BeEquivalentTo(resourceResults);
        }

        /// <summary>
        ///     This method tests the FetchAllPublicGithubRepositories method in a bad flow where the http status
        ///     code from the response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public void FetchAllPublicGithubRepositories_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(new GithubDataSourceResourceResult[0], HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = new GithubDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchAllPublicGithubRepositories(It.IsAny<string>());

            // Assert
            act.Should().ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        /// <summary>
        ///     This method tests the FetchPublicRepository method in a good flow. In this scenario a github data source
        ///     exists and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct resource result.</returns>
        [Test]
        public async Task FetchPublicRepository_GoodFlow(
            [GithubDataSourceResourceResultDataSource] GithubDataSourceResourceResult resourceResult)
        {
            // Arrange
            MockRestClient(resourceResult, HttpStatusCode.OK);
            DataSourceAdaptee = new GithubDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Uri testUri = new Uri("https://google.nl/test");
            Action act = () => DataSourceAdaptee.FetchPublicRepository(testUri);
            GithubDataSourceResourceResult retrievedResourceResult = await DataSourceAdaptee.FetchPublicRepository(testUri);

            // Assert
            act.Should().NotThrow();
            retrievedResourceResult.Should().BeEquivalentTo(resourceResult);
        }

        /// <summary>
        ///     This method tests the FetchPublicRepository method in a bad flow where the http status
        ///     code from the response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public void FetchPublicRepository_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null , HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = new GithubDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Uri testUri = new Uri("https://google.nl/test");
            Func<Task> act = () => DataSourceAdaptee.FetchPublicRepository(testUri);

            // Assert
            act.Should().ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        /// <summary>
        ///     This method tests the FetchPublicGithubProjectById method in a good flow. In this scenario
        ///     a github data source with the specified identifier exists and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct resource result.</returns>
        [Test]
        public async Task FetchPublicGithubProjectById_GoodFlow(
            [GithubDataSourceResourceResultDataSource] GithubDataSourceResourceResult resourceResult)
        {
            // Arrange
            MockRestClient(resourceResult, HttpStatusCode.OK);
            DataSourceAdaptee = new GithubDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchPublicGithubProjectById(It.IsAny<string>());
            GithubDataSourceResourceResult retrievedResourceResult = await DataSourceAdaptee.FetchPublicGithubProjectById(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedResourceResult.Should().BeEquivalentTo(resourceResult);
        }

        /// <summary>
        ///     This method tests the FetchPublicGithubProjectById method in a bad flow where the http status
        ///     code from the response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public void FetchPublicGithubProjectById_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null , HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = new GithubDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchPublicGithubProjectById(It.IsAny<string>());

            // Assert
            act.Should().ThrowExactly<ExternalException>()
               .WithMessage(errorMessage);
        }

        /// <summary>
        ///     This method tests the FetchReadme method in a good flow. In this scenario
        ///     a readme github data source is found and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct readme resource result.</returns>
        [Test]
        public async Task FetchReadme_GoodFlow(
            [GithubDataSourceReadmeResourceResultDataSource] GithubDataSourceReadmeResourceResult resourceResult)
        {
            // Arrange
            MockRestClient(resourceResult, HttpStatusCode.OK);
            DataSourceAdaptee = new GithubDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchReadme(It.IsAny<string>(),It.IsAny<string>());
            string retrievedResourceResult = await DataSourceAdaptee.FetchReadme(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedResourceResult.Should().BeEquivalentTo(JsonConvert.SerializeObject(resourceResult));
        }

        /// <summary>
        ///     This method tests the FetchReadme method in a flow where the readme could not be found.
        /// </summary>
        /// <returns>The tested method will receive a 404 Not Found response from the external API.</returns>
        [Test]
        public async Task FetchReadme_ReadmeNotFound()
        {
            // Arrange
            MockRestClient(null , HttpStatusCode.NotFound);
            DataSourceAdaptee = new GithubDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchReadme(It.IsAny<string>(), It.IsAny<string>());
            string retrievedReadme = await DataSourceAdaptee.FetchReadme(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedReadme.Should().BeNullOrEmpty();
        }

        /// <summary>
        ///     This method tests the FetchReadme method in a bad flow where the http status
        ///     code from the response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public void FetchReadme_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null , HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = new GithubDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchReadme(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            act.Should().ThrowExactly<ExternalException>().WithMessage(errorMessage);
        }

        /// <summary>
        ///     This method tests the FetchReadmeContent method in a good flow. In this scenario
        ///     a readme content is found and will get returned.
        /// </summary>
        /// <returns>The tested method will return the correct readme content.</returns>
        [Test]
        public async Task FetchReadmeContent_GoodFlow()
        {
            // Arrange
            string readmeContent = new string("This is the content from a test readme file");
            MockRestClient(readmeContent, HttpStatusCode.OK);
            DataSourceAdaptee = new GithubDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Action act = () => DataSourceAdaptee.FetchReadmeContent(It.IsAny<Uri>());
            string retrievedResourceResult = await DataSourceAdaptee.FetchReadmeContent(It.IsAny<Uri>());

            // Assert
            act.Should().NotThrow();
            retrievedResourceResult.Substring(1, retrievedResourceResult.Length-2).Should().BeEquivalentTo(readmeContent);
        }

        /// <summary>
        ///     This method tests the FetchReadmeContent method in a flow where the readme content could not be found.
        /// </summary>
        /// <returns>The tested method will receive a 404 Not Found response from the external API.</returns>
        [Test]
        public async Task FetchReadmeContent_ContentNotFound()
        {
            // Arrange
            MockRestClient(null , HttpStatusCode.NotFound);
            DataSourceAdaptee = new GithubDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchReadmeContent(It.IsAny<Uri>());
            string retrievedReadmeContent = await DataSourceAdaptee.FetchReadmeContent(It.IsAny<Uri>());

            // Assert
            act.Should().NotThrow();
            retrievedReadmeContent.Should().BeNullOrEmpty();
        }

        /// <summary>
        ///     This method tests the FetchReadmeContent method in a bad flow where the http status
        ///     code from the response is not successful.
        /// </summary>
        /// <returns>The tested method will receive a not successful response from the external API.</returns>
        [Test]
        public void FetchReadmeContent_ResponseIsNotSuccessful()
        {
            // Arrange
            string errorMessage = "Invalid test request";
            MockRestClient(null , HttpStatusCode.BadRequest, errorMessage);
            DataSourceAdaptee = new GithubDataSourceAdaptee(ConfigurationMock, ClientFactoryMock.Object, Mapper);

            // Act
            Func<Task> act = () => DataSourceAdaptee.FetchReadmeContent(It.IsAny<Uri>());

            // Assert
            act.Should().ThrowExactly<ExternalException>().WithMessage(errorMessage);
        }

    }

}
