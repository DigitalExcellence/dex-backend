using IntegrationTests.Base;
using IntegrationTests.Test.Base;
using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Test
{
    public class UpdateTest : BaseTest
    {
        protected dynamic UpdateResource;

        public UpdateTest(BaseACLTestCollection collection, Type expectedResultType, dynamic updateResource) : base(collection, expectedResultType)
        {
            UpdateResource = updateResource;
        }

        public override async Task Execute()
        {
            await Update();
        }

        private async Task Update()
        {
            // Arrange
            var client = Collection.Connection.Client;
            var httpHelper = Collection.HttpHelper;
            var endpoint = Collection.Endpoint;
            var content = httpHelper.GetHttpContent(UpdateResource);
            var id = Collection.CreatedId;

            // Act
            var response = await client.PutAsync(endpoint + $"/{id}", content);

            // Assert
            dynamic responseObj = await httpHelper.GetFromResponse(response, ExpectedResultType);
            response.EnsureSuccessStatusCode();
            Assert.Equal<int>(responseObj.Id, id);
        }
    }
}
