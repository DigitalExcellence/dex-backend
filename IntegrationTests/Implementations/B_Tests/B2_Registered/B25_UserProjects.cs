using API.Resources;
using IntegrationTests.Settings;
using IntegrationTests.Test.Templates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace IntegrationTests.Implementations.B_Tests.B2_Registered
{
    [Collection("Sequential")]
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class B25_UserProjects
    {
        private HttpConnection connection;
        private HttpHelper httpHelper;
        public B25_UserProjects(DatabaseFixture fixture)
        {
            this.connection = fixture.Connection;
            this.httpHelper = fixture.HttpHelper;
        }

        [Fact, Priority(10)]

        public async Task Test1()
        {
            string identityId = EnvironmentVariables.Variables["RegisteredUser"];

            //?
            await connection.ApplyAuthenticationToClient(identityId);

            var requestConfig = new RequestConfig
            {
                Connection = connection,
                Endpoint = "/api/Project",
                HttpHelper = httpHelper
            };
            //

            var collaborators = new List<CollaboratorInput>
            {
                new CollaboratorInput()
                {
                    FullName = "postmantest_UserProject-CreateProject-Registered",
                    Role = "postmantest_UserProject-CreateProject-Registered"
                }
            };

            var expectedResultType = typeof(ProjectOutput);
            var createResource = new ProjectInput
            {
                Name = "postmantest_UserProject-CreateProject-Registered",
                Description = "postmantest_UserProject-CreateProject-Registered",
                ShortDescription = "postmantest_UserProject-CreateProject-Registered",
                Uri = "postmantest_UserProject-CreateProject-Registered",
                Collaborators = collaborators,
            };

            var test = new CreateTest(requestConfig, expectedResultType, createResource);
            await test.Execute();

            EnvironmentVariables.Variables["UserProjectId1"] = test.Result.Id.ToString();
            await test.Execute();
            EnvironmentVariables.Variables["UserProjectId2"] = test.Result.Id.ToString();

        }

        [Fact, Priority(20)]
        public async Task Test2()
        {
            string identityId = EnvironmentVariables.Variables["RegisteredUser"];

            await connection.ApplyAuthenticationToClient(identityId);

            var expectedOutput = typeof(ProjectResultsInput);
            var requestConfig = new RequestConfig
            {
                Connection = connection,
                Endpoint = "/api/user/projects?amountOnPage=1&page=1",
                HttpHelper = httpHelper
            };
            var checkingId = Convert.ToInt32(EnvironmentVariables.Variables["UserProjectId2"]);
            var test = new GetAllContainsTest(requestConfig, expectedOutput, checkingId);

            await test.Execute();

            var requestConfig2 = new RequestConfig
            {
                Connection = connection,
                Endpoint = "/api/user/projects?amountOnPage=1&page=2",
                HttpHelper = httpHelper
            };

            var checkingId2 = Convert.ToInt32(EnvironmentVariables.Variables["UserProjectId1"]);
            var test2 = new GetAllContainsTest(requestConfig2, expectedOutput, checkingId2);

            await test2.Execute();
        }

        [Fact, Priority(30)]
        public async Task Test3()
        {
            string identityId = EnvironmentVariables.Variables["RegisteredUser"];

            var userProjectId2 = EnvironmentVariables.Variables["UserProjectId2"];

            string endpoint = "/api/Project/" + EnvironmentVariables.Variables["UserProjectId1"];
            await connection.ApplyAuthenticationToClient(identityId);

            var requestConfig = new RequestConfig
            {
                Connection = connection,
                Endpoint = endpoint,
                HttpHelper = httpHelper
            };
            var test = new DeleteTest(requestConfig);

            await test.Execute();

            string endpoint2 = "/api/Project/" + EnvironmentVariables.Variables["UserProjectId2"];
            var requestConfig2 = new RequestConfig
            {
                Connection = connection,
                Endpoint = endpoint2,
                HttpHelper = httpHelper
            };
            var test2 = new DeleteTest(requestConfig2);
            await test2.Execute();
        }


    }
}
