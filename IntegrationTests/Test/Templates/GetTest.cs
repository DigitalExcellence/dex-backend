using IntegrationTests.Settings;
using IntegrationTests.Test.Base;
using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Test.Templates
{
    public class GetTest : BaseTest
    {
        protected string[] CheckProperties;
        protected dynamic CheckingResource;

        public GetTest(RequestConfig requestConfig, Type expectedResultType, string[] checkProperties, dynamic checkingResource) : base(requestConfig, expectedResultType)
        {
            CheckProperties = checkProperties;
            CheckingResource = checkingResource;
        }

        public override async Task Execute()
        {
            await Get();
        }

        private async Task Get()
        {
            // Arrange
            var client = RequestConfig.Connection.Client;
            var endpoint = RequestConfig.Endpoint;
            var httpHelper = RequestConfig.HttpHelper;
            var id = CheckingResource.Id;

            // Act
            var response = await client.GetAsync(endpoint + $"/{id}");

            // Assert
            dynamic responseObj = await httpHelper.GetFromResponse(response, ExpectedResultType);
            response.EnsureSuccessStatusCode();
            Assert.True((int) responseObj.Id == id);

            foreach(string property in CheckProperties)
            {
                Assert.Equal(GetProperty(CheckingResource, property), GetProperty(responseObj, property));
            }
        }

        private dynamic GetProperty(dynamic obj, string property)
        {
            return obj.GetType().GetProperty(property).GetValue(obj);
        }
    }
}
