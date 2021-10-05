using IntegrationTests.Settings;
using IntegrationTests.Test.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Test.Templates
{
    public class DeleteTest : BaseTest
    {

        public DeleteTest(RequestConfig requestConfig) : base (requestConfig, null)
        {

        }

        public override async Task Execute()
        {
            await Delete();
        }

        private async Task Delete()
        {
            //Arrange
            var client = RequestConfig.Connection.Client;
            var endpoint = RequestConfig.Endpoint;

            // Act
            var response = await client.DeleteAsync(endpoint);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
