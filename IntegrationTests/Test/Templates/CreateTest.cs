using IntegrationTests.Base;
using IntegrationTests.Settings;
using IntegrationTests.Test.Base;
using System;
using System.Threading.Tasks;

namespace IntegrationTests.Test.Templates
{
    public class CreateTest : BaseTest
    {
        public dynamic Result { get; set; }

        protected dynamic CreateResource;

        public CreateTest(RequestConfig requestConfig, Type expectedResultType, dynamic createResource) : base(requestConfig, expectedResultType)
        {
            CreateResource = createResource;
        }

        public override async Task Execute()
        {
            await Create();
        }

        private async Task Create()
        {
            // Arrange
            var httpHelper = RequestConfig.HttpHelper;
            var endpoint = RequestConfig.Endpoint;
            var client = RequestConfig.Connection.Client;
            var content = httpHelper.GetHttpContent(CreateResource);

            // Act
            var response = await client.PostAsync(endpoint, content);

            // Assert
            dynamic responseObj = await httpHelper.GetFromResponse(response, ExpectedResultType);
            Result = responseObj;
            response.EnsureSuccessStatusCode();
        }
    }
}
