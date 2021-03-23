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
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Services.ExternalDataProviders;
using Services.Sources;
using Services.Tests.Helpers;
using System;
using System.Net;
using System.Threading;

namespace Services.Tests.Base
{

    public abstract class AdapteeTest<TInterface>
        where TInterface : IDataSourceAdaptee
    {

        private readonly Mock<IRestClient> clientMock;

        protected TInterface DataSourceAdaptee;

        /// <summary>
        ///     A mock of the configuration.
        /// </summary>
        protected IConfiguration ConfigurationMock;

        /// <summary>
        ///     The actual mapper.
        /// </summary>
        protected readonly IMapper Mapper;

        protected readonly Mock<IRestClientFactory> ClientFactoryMock;

        protected AdapteeTest()
        {
            Mapper = AutoMapperServiceTestHelper.GetIMapper();

            clientMock = new Mock<IRestClient>();

            ClientFactoryMock = new Mock<IRestClientFactory>();
            ClientFactoryMock
                .Setup(_ => _.Create(It.IsAny<Uri>()))
                .Returns(clientMock.Object);
        }

        protected void MockRestClient(object result, HttpStatusCode statusCode, string errorMessage = null)
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
