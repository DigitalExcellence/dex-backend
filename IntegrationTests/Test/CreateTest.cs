using IntegrationTests.Base;
using IntegrationTests.Test.Base;
using System;
using System.Threading.Tasks;

namespace IntegrationTests.Test
{
    public class CreateTest : BaseTest
    {
        protected dynamic CreateResource;

        public CreateTest(BaseACLTestCollection collection, Type expectedResultType, dynamic createResource) : base(collection, expectedResultType)
        {
            CreateResource = createResource;
        }

        public override async Task Execute()
        {
            int createdId = await Create();
            Collection.CreatedId = createdId;
        }

        private async Task<int> Create()
        {
            // Arrange
            var httpHelper = Collection.HttpHelper;
            var endpoint = Collection.Endpoint;
            var client = Collection.Connection.Client;
            var content = httpHelper.GetHttpContent(CreateResource);

            // Act
            var response = await client.PostAsync(endpoint, content);

            // Assert
            dynamic responseObj = await httpHelper.GetFromResponse(response, ExpectedResultType);
            response.EnsureSuccessStatusCode();
            return (int) responseObj.Id;
        }
    }
}
