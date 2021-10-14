using IntegrationTests.Base;
using IntegrationTests.Settings;
using IntegrationTests.Test.Base;
using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Test.Templates
{
    public class UpdateTest : BaseTest
    {
        protected dynamic UpdateResource;
        protected int CreatedId;

        public UpdateTest(RequestConfig requestConfig, Type expectedResultType, dynamic updateResource) : base(requestConfig, expectedResultType)
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
            var client = RequestConfig.Connection.Client;
            var httpHelper = RequestConfig.HttpHelper;
            var endpoint = RequestConfig.Endpoint;
            var content = httpHelper.GetHttpContent(UpdateResource);
            var id = CreatedId;

            // Act
            var response = await client.PutAsync(endpoint + $"/{id}", content);

            // Assert
            dynamic responseObj = await httpHelper.GetFromResponse(response, ExpectedResultType);
            response.EnsureSuccessStatusCode();
            Assert.Equal<int>(responseObj.Id, id);
        }
    }
}
