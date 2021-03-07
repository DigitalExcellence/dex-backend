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
using System.Net.Http;
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
            Dictionary<string, string> inMemoryConf = new Dictionary<string, string>()
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

            var client = clientFactoryMock.Object.Create(new Uri("https://localhost:5001"));
            var response = await client.ExecuteAsync(new RestRequest());
            var success = response.IsSuccessful;

            // Act
            Action act = () => dataSourceAdaptee.FetchAllGithubProjects(It.IsAny<string>());
            IEnumerable<GithubDataSourceResourceResult> retrievedResourceResults = await dataSourceAdaptee.FetchAllGithubProjects(It.IsAny<string>());

            // Assert
            act.Should().NotThrow();
            retrievedResourceResults.Should().BeEquivalentTo(resourceResults);

        }

        private void MockRestClient(IEnumerable<GithubDataSourceResourceResult> resourceResults, HttpStatusCode statusCode)
        {
            Mock<IRestResponse> response = new Mock<IRestResponse>();
            response.Setup(_ => _.Content)
                    .Returns(JsonConvert.SerializeObject(resourceResults));

            response.Setup(_ => _.StatusCode)
                    .Returns(statusCode);

            response.Setup(_ => _.IsSuccessful)
                    .Returns(() => (int) statusCode >= 200 && (int) statusCode <= 299);

            clientMock.Setup(_ => _.ExecuteAsync(It.IsAny<IRestRequest>(), CancellationToken.None))
                      .ReturnsAsync(response.Object);
        }


    }

}
