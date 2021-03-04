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
using NUnit.Framework;
using RestSharp;
using Services.ExternalDataProviders;
using Services.Sources;
using System;
using System.Net;
using System.Reflection;

namespace Services.Tests.Base
{

    public abstract class DataSourceAdapteeTest<TAdaptee>
        where TAdaptee : IDataSourceAdaptee
    {

        /// <summary>
        ///     The IDataSourceAdaptee is the data source adaptee that will get tested.
        /// </summary>
        protected IDataSourceAdaptee DataSourceAdaptee;

        /// <summary>
        ///     A mock of the configuration.
        /// </summary>
        private Mock<IConfiguration> configurationMock;

        /// <summary>
        ///     A mock of the rest client factory.
        /// </summary>
        private Mock<IRestClientFactory> restClientFactoryMock;

        /// <summary>
        ///     A mock of the rest client that the rest client factory will create.
        /// </summary>
        private Mock<IRestClient> restClient;

        /// <summary>
        ///     The actual mapper.
        /// </summary>
        private IMapper mapper;


        [SetUp]
        public void Initialize()
        {
            configurationMock = new Mock<IConfiguration>();

            restClient = new Mock<IRestClient>();

            restClientFactoryMock = new Mock<IRestClientFactory>();
            restClientFactoryMock.Setup(_ => _.Create(It.IsAny<Uri>())).Returns(restClient.Object);

            MapperConfiguration mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
            mapper = mappingConfig.CreateMapper();

            // Create adaptee with reflection.
            Type adapteeType = typeof(TAdaptee);
            ConstructorInfo adapteeCtor =
                adapteeType.GetConstructor(new[] {typeof(IConfiguration), typeof(IRestClientFactory), typeof(IMapper)});
            DataSourceAdaptee =
                (TAdaptee) adapteeCtor.Invoke(new object[]
                                              {
                                                  configurationMock.Object, restClientFactoryMock.Object, mapper
                                              });
        }

        /// <summary>
        ///     Set the correct settings for the mocked rest client.
        /// </summary>
        /// <param name="statusCode">The http status code that will get returned.</param>
        /// <param name="data">The content from the IRestResponse.</param>
        protected void MockRestClient(HttpStatusCode statusCode, string data)
        {
            Mock<IRestResponse> response = new Mock<IRestResponse>();
            response.Setup(_ => _.StatusCode)
                    .Returns(statusCode);

            response.Setup(_ => _.Content)
                    .Returns(data);

            restClient.Setup(_ => _.Execute(It.IsAny<IRestRequest>()))
                      .Returns(response.Object);
        }

    }

}
