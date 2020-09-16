using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Repositories.Base;
using RestSharp;
using Services.Base;
using Services.Sources;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Services.Tests.Base
{
    /// <summary>
    /// Base test class which should be inherited from when creating unittests for the sources.
    /// By inheriting, all the default tests are included in the new test class.
    /// YOU SHOULD OVERRIDE THE DEFAULT TEST TO ADD THE [Test] attribute.
    /// If you do not override the tests and add the [Test] attribute, the default tests will not be triggered.
    /// </summary>
    /// <typeparam name="TSource">Source which should be tested</typeparam>
    public abstract class SourceTest<TSource>
        where TSource : class
    {
        protected TSource Source;
        protected Mock<IRestClientFactory> RestClientFactoryMock;

        /// <summary>
        /// Initialize runs before every test
        /// Mock the rest client factory
        /// Initialize the service with reflection
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            // Mock the repository
            RestClientFactoryMock = new Mock<IRestClientFactory>();

            // Create the service with reflection
            Type sourceType = typeof(TSource);
            ConstructorInfo sourceCtor = sourceType.GetConstructor(new[] { typeof(IRestClientFactory) });
            Source = (TSource) sourceCtor.Invoke(new object[] { RestClientFactoryMock.Object });
        }

        /// <summary>
        /// Mocks the rest client.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="data">The irestresponse content.</param>
        /// <returns>mocked IRestResponse.</returns>
        public static IRestClient MockRestClient(HttpStatusCode httpStatusCode, string data)
        {
            Mock<IRestResponse> response = new Mock<IRestResponse>();
            response.Setup(_ => _.StatusCode).Returns(httpStatusCode);
            response.Setup(_ => _.Content).Returns(data);

            Mock<IRestClient> mockIRestClient = new Mock<IRestClient>();
            mockIRestClient
                .Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(response.Object);
            return mockIRestClient.Object;
        }
    }
}
