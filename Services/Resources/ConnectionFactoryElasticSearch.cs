using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Services.Resources
{
    public interface IConnectionFactoryElasticSearch
    {
        RestClient CreateRestClientForElasticRequests();
    }
    public class ConnectionFactoryElasticSearch : IConnectionFactoryElasticSearch
    {
        private Uri uri;
        private ElasticConfig config;

        public ConnectionFactoryElasticSearch(ElasticConfig config)
        {
            this.config = config;
        }

        public RestClient CreateRestClientForElasticRequests()
        {
            UriBuilder builder = new UriBuilder(Dns.GetHostEntry(config.Hostname).AddressList.FirstOrDefault().ToString() + ":9200");
            Uri uri = builder.Uri;
            RestClient restClient = new RestClient(uri)
            {
                Authenticator =
                                 new HttpBasicAuthenticator(config.Username, config.Password)
            };
            return restClient;
        }
    }
}
