using IntegrationTests.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Implementations
{
    public class DatabaseFixture
    {
        public HttpConnection Connection { get; private set; }
        public HttpHelper HttpHelper { get; private set; }

        public DatabaseFixture()
        {
            var factory = new HttpClientFactory();

            HttpHelper = new HttpHelper();
            Connection = new HttpConnection
            {
                Client = factory.CreateClient(),
                Factory = factory
            };
        }

        [CollectionDefinition("Sequential")]
        public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
        {
            // This class has no code, and is never created. Its purpose is simply
            // to be the place to apply [CollectionDefinition] and all the
            // ICollectionFixture<> interfaces.
        }
    }
}
