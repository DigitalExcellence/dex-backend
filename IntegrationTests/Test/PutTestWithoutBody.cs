using IntegrationTests.Base;
using IntegrationTests.Test.Base;
using System;
using System.Threading.Tasks;

namespace IntegrationTests.Test
{
    public class PutTestWithoutBody : BaseTest
    {
        public PutTestWithoutBody(BaseTestCollection collection, Type expectedResultType) : base(collection, expectedResultType)
        {
        }

        public override async Task Execute()
        {
            await Put();
        }

        private async Task Put()
        {
            // Arrange
            var client = Collection.Connection.Client;
            var httpHelper = Collection.HttpHelper;
            var content = httpHelper.GetHttpContent(null);
            var id = Collection.CreatedId;

            // Act
            var response = await client.PutAsync(Endpoint, content);

            // Assert
            dynamic responseObj = await httpHelper.GetFromResponse(response, ExpectedResultType);
            response.EnsureSuccessStatusCode();
        }
    }
}
