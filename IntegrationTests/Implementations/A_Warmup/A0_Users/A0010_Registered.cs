using API.Resources;
using IntegrationTests.Settings;
using IntegrationTests.Test.Templates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Implementations.A_Warmup.A0_Users
{
    [Collection("Sequential")]
    public class A0010_Registered
    {
        public string IdentityId { get; protected set; } = "88421113";
        private HttpConnection connection;
        private HttpHelper httpHelper;

        public A0010_Registered(DatabaseFixture fixture)
        {
            this.connection = fixture.Connection;
            this.httpHelper = fixture.HttpHelper;
        }

        [Fact]
        public async Task CreateRegisteredUser()
        {
            string identityId = "818727";

            string name = "Registered Postman User";
            string email = "registeredUser@postman.com";
            int institutionId = 1;

            //?
            await connection.ApplyAuthenticationToClient(IdentityId);

            var requestConfig = new RequestConfig
            {
                Connection = connection,
                Endpoint = "/api/User",
                HttpHelper = httpHelper
            };
            //


            var expectedResultType = typeof(UserOutput);
            var createResource = new UserInput
            {
                IdentityId = identityId,
                Name = name,
                Email = email,
                InstitutionId = institutionId
            };

            var test = new CreateTest(requestConfig, expectedResultType, createResource);
            await test.Execute();

            //Naam controleren? ID ergens opslaan voor gebruik andere testen?
            Assert.Equal(name, test.Result.Name);
        }
    }
}
