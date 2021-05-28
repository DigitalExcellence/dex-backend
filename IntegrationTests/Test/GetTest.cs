using IntegrationTests.Base;
using IntegrationTests.Test.Base;
using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Test
{
    public class GetTest : BaseTest
    {
        protected string[] CheckProperties;
        protected dynamic CheckingResource;

        public GetTest(BaseACLTestCollection collection, Type expectedResultType, string[] checkProperties, dynamic checkingResource) : base(collection, expectedResultType)
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
            var client = Collection.Connection.Client;
            var endpoint = Collection.Endpoint;
            var httpHelper = Collection.HttpHelper;
            var id = Collection.CreatedId;

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
