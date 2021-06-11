using IntegrationTests.Base;
using IntegrationTests.Settings;
using IntegrationTests.Test.Base;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Test.Templates
{
    public class GetAllContainsTest : BaseTest
    {
        public int CheckingId { get; set; }

        public GetAllContainsTest(RequestConfig requestConfig, Type expectedResultType, int checkingId) : base(requestConfig, expectedResultType)
        {
            CheckingId = checkingId;
        }

        public override async Task Execute()
        {
            await GetAllCreated();
        }

        private async Task GetAllCreated()
        {
            // Arrange
            var client = RequestConfig.Connection.Client;
            var endpoint = RequestConfig.Endpoint;
            var httpHelper = RequestConfig.HttpHelper;

            // Act
            var response = await client.GetAsync(endpoint);

            // Assert
            dynamic responseObj = await httpHelper.GetFromResponse(response, ExpectedResultType);
            dynamic[] results = (dynamic[]) responseObj.Results;
            int count = results.Where(d => d.Id == CheckingId).Count();

            response.EnsureSuccessStatusCode();
            Assert.True(count >= 1);
        }
    }
}
