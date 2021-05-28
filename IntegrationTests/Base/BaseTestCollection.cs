using IntegrationTests.Settings;
using IntegrationTests.Test.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Base
{
    public abstract class BaseTestCollection : IClassFixture<HttpClientFactory>, IClassFixture<HttpHelper>, IAsyncLifetime
    {
        public HttpConnection Connection;
        public HttpHelper HttpHelper;
        public string IdentityId = "88421113";

        protected LinkedList<BaseTest> Tests;

        public BaseTestCollection()
        {
            Connection = new HttpClientFactory().GetConnection();
            HttpHelper = new HttpHelper();
        }


        //[Fact]
        //public virtual async Task ACLTest()
        //{
        //    foreach(BaseTest test in Tests)
        //    {
        //        await test.Execute();
        //    }
        //}

        public async Task InitializeAsync()
        {
            await Connection.ApplyAuthenticationToClient(IdentityId);
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
