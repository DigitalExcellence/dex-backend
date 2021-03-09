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
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using Services.ExternalDataProviders;
using Services.Sources;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Tests.ExternalDataProviders
{

    [TestFixture]
    public class JsFiddleDataSourceAdapteeTest
    {

        /// <summary>
        ///     The IDataSourceAdaptee which in this case is the IGithubDataSourceAdaptee.
        /// </summary>
        private IJsFiddleDataSourceAdaptee dataSourceAdaptee;

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
                                                          {"App:DataSources:JsFiddle", "valid_access_token"}
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
        public async Task FetchAllFiddlesFromUser_GoodFlow()
        {
            // Arrange
            

            // Act


            // Assert


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
