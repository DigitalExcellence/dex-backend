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

using Moq;
using NUnit.Framework;
using RestSharp;
using Services.Sources;
using System;
using System.Net;
using System.Reflection;

namespace Services.Tests.Base
{

    /// <summary>
    ///     Base test class which should be inherited from when creating unittests for the sources.
    ///     By inheriting, all the default tests are included in the new test class.
    ///     YOU SHOULD OVERRIDE THE DEFAULT TEST TO ADD THE [Test] attribute.
    ///     If you do not override the tests and add the [Test] attribute, the default tests will not be triggered.
    /// </summary>
    /// <typeparam name="TSource">Source which should be tested</typeparam>
    public abstract class SourceTest<TSource>
        where TSource : class
    {

        protected Mock<IRestClientFactory> RestClientFactoryMock;
        protected TSource Source;

        /// <summary>
        ///     Initialize runs before every test
        ///     Mock the rest client factory
        ///     Initialize the service with reflection
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            // Mock the repository
            RestClientFactoryMock = new Mock<IRestClientFactory>();

            // Create the service with reflection
            Type sourceType = typeof(TSource);
            ConstructorInfo sourceCtor = sourceType.GetConstructor(new[] {typeof(IRestClientFactory)});
            Source = (TSource) sourceCtor.Invoke(new object[] {RestClientFactoryMock.Object});
        }

        /// <summary>
        ///     Mocks the rest client.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="data">The irestresponse content.</param>
        /// <returns>mocked IRestResponse.</returns>
        public static IRestClient MockRestClient(HttpStatusCode httpStatusCode, string data)
        {
            Mock<IRestResponse> response = new Mock<IRestResponse>();
            response.Setup(_ => _.StatusCode)
                    .Returns(httpStatusCode);
            response.Setup(_ => _.Content)
                    .Returns(data);

            Mock<IRestClient> mockIRestClient = new Mock<IRestClient>();
            mockIRestClient
                .Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(response.Object);
            return mockIRestClient.Object;
        }

    }

}
