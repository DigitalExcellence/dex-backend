using IntegrationTests.Settings;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Base
{
    public abstract class BaseTest : IClassFixture<HttpClientFactory>, IClassFixture<HttpHelper>, IAsyncLifetime
    {
        protected HttpConnection Connection;
        protected HttpHelper HttpHelper;

        public BaseTest()
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
