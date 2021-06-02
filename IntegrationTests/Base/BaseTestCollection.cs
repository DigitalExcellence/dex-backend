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
        public string Endpoint { get; set; }
        public int CreatedId { get; set; }
        public string IdentityId { get; protected set; } = "88421113";
        public bool Authorize { get; protected set; } = true;

        protected LinkedList<BaseTest> Tests;

        public BaseTestCollection()
        {
            Connection = new HttpClientFactory().GetConnection();
            HttpHelper = new HttpHelper();
        }

        public virtual async Task InitializeAsync()
        {
            if(Authorize)
            {
                await Connection.ApplyAuthenticationToClient(IdentityId);
            }
        }

        public virtual Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
