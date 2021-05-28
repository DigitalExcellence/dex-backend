using IntegrationTests.Settings;
using IntegrationTests.Test.Base;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Base
{
    public abstract class BaseTestCollection : IClassFixture<HttpClientFactory>, IClassFixture<HttpHelper>, IAsyncLifetime
    {
        public HttpConnection Connection;
        public HttpHelper HttpHelper;

        protected BaseTest[] Tests;

        public BaseTestCollection()
        {
            this.Connection = new HttpClientFactory().GetConnection();
            this.HttpHelper = new HttpHelper();
        }

        //public BaseTest(HttpClientFactory factory, HttpHelper httpHelper)
        //{
        //    this.Connection = factory.GetConnection();
        //    this.HttpHelper = httpHelper;
        //}

        public async Task InitializeAsync()
        {
            await Connection.ApplyAuthenticationToClient();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
