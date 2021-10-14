using IntegrationTests.Settings;
using IntegrationTests.Test.Templates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace IntegrationTests.Implementations.C_CleanUp
{
    [Collection("Sequential")]
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class DeleteUsers
    {
        private HttpConnection connection;
        private HttpHelper httpHelper;
        public DeleteUsers(DatabaseFixture fixture)
        {
            this.connection = fixture.Connection;
            this.httpHelper = fixture.HttpHelper;
        }

        [Fact, Priority(90)]
        public async Task RemoveUser()
        {
            //Administrator user id
            string identityId = "88421113";

            string endpoint = "/api/User/" + EnvironmentVariables.Variables["RegisteredUserId"];
            await connection.ApplyAuthenticationToClient(identityId);

            var requestConfig = new RequestConfig
            {
                Connection = connection,
                Endpoint = endpoint,
                HttpHelper = httpHelper
            };
            var test = new DeleteTest(requestConfig);

            await test.Execute();
        }
    }
}
