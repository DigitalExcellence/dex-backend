using IntegrationTests.Base;
using IntegrationTests.Settings;
using IntegrationTests.Test.Base;
using System;
using System.Threading.Tasks;

namespace IntegrationTests.Test.Templates
{
    public class PutTestWithoutBody : BaseTest
    {
        public int Id { get; set; }

        public PutTestWithoutBody(RequestConfig requestConfig, Type expectedResultType, int id) : base(requestConfig, expectedResultType)
        {
            Id = id;
        }

        public override async Task Execute()
        {
            await Put();
        }

        private async Task Put()
        {
            // Arrange
            var client = RequestConfig.Connection.Client;
            var httpHelper = RequestConfig.HttpHelper;
            var content = httpHelper.GetHttpContent(null);

            // Act
            var response = await client.PutAsync(Endpoint, content);

            // Assert
            dynamic responseObj = await httpHelper.GetFromResponse(response, ExpectedResultType);
            response.EnsureSuccessStatusCode();
        }
    }
}
