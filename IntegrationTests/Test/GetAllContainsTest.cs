using IntegrationTests.Base;
using IntegrationTests.Test.Base;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Test
{
    public class GetAllContainsTest : BaseTest
    {
        public GetAllContainsTest(BaseACLTestCollection collection, Type expectedResultType) : base(collection, expectedResultType)
        {

        }

        public override async Task Execute()
        {
            await GetAllCreated();
        }

        private async Task GetAllCreated()
        {
            // Arrange
            var client = Collection.Connection.Client;
            var endpoint = Collection.Endpoint;
            var httpHelper = Collection.HttpHelper;
            var id = Collection.CreatedId;

            // Act
            var response = await client.GetAsync(endpoint);

            // Assert
            dynamic responseObj = await httpHelper.GetFromResponse(response, ExpectedResultType);
            dynamic[] results = (dynamic[]) responseObj.Results;
            int count = results.Where(d => d.Id == id).Count();

            response.EnsureSuccessStatusCode();
            Assert.True(count >= 1);
        }
    }
}
