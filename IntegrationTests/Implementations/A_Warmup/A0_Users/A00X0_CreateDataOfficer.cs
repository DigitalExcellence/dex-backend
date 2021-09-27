using API.Resources;
using IntegrationTests.Settings;
using IntegrationTests.Test.Templates;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Implementations.A_Warmup.A0_Users
{
    [Collection("Sequential")]
    public class A00X0_CreateDataOfficer
    {
        public string IdentityId { get; protected set; } = "88421113";
        private HttpConnection connection;
        private HttpHelper httpHelper;

        public A00X0_CreateDataOfficer(DatabaseFixture fixture)
        {
            this.connection = fixture.Connection;
            this.httpHelper = fixture.HttpHelper;
        }

        //Test is niet juiste (hoort niet in warmup)

        [Fact]

        public async Task Test()
        {
            string userName = "postmantest_username_updated";

            await connection.ApplyAuthenticationToClient(IdentityId);

            var requestConfig = new RequestConfig
            {
                Connection = connection,
                Endpoint = "/api/User",
                HttpHelper = httpHelper
            };

            var expectedResultType = typeof(UserOutput);
            var createResource = new UserInput
            {
                IdentityId = "999",
                Name = userName,
                Email = "User-CreateUser-Administrator@example.com",
            };

            var test = new CreateTest(requestConfig, expectedResultType, createResource);
            await test.Execute();

            Assert.Equal(userName, test.Result.Name);
        }
    }
}
