using API.Resources;
using IntegrationTests.Settings;
using IntegrationTests.Test.Templates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Bogus;

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
            connection = fixture.Connection;
            httpHelper = fixture.HttpHelper;
        }

        [Fact]
        public async Task CreateRegisteredUser()
        {
            //AdministratorId
            string identityId = "818727";

            string name = "Registered Postman User";
            string email = "registeredUser@postman.com";
            int institutionId = 1;

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
                IdentityId = identityId,
                Name = name,
                Email = email,
                InstitutionId = institutionId
            };
            
            var test = new CreateTest(requestConfig, expectedResultType, createResource);
            await test.Execute();

            Assert.Equal(name, test.Result.Name);
            Console.WriteLine("testing");
        }

        [Fact]
        public async Task RegisteredUser_CanCreateProject_Returns_Created()
        {
            string identityId = "818727";

            await connection.ApplyAuthenticationToClient(identityId);

            var requestConfig = new RequestConfig
            {
                Connection = connection,
                Endpoint = "/api/Project",
                HttpHelper = httpHelper
            };

            var expectedResultType = typeof(ProjectOutput);
            var createResource = new Faker<ProjectInput>()
                    .RuleFor(project => project.Name, faker => faker.Name.FirstName())
                    .RuleFor(project => project.ShortDescription,
                             faker => faker.Lorem.Words(10)
                                           .ToString())
                    .RuleFor(project => project.Description,
                             faker => faker.Lorem.Words(40)
                                           .ToString())
                    .RuleFor(project => project.Uri, faker => faker.Internet.Url());

            var test = new CreateTest(requestConfig, expectedResultType, createResource);

            await test.Execute();
        }
    }
}
