using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Data
{
    public interface IElasticSearchContext
    {
        RestClient CreateRestClientForElasticRequests();
    }
    public class ElasticSearchContext : IElasticSearchContext
    {
        private string hostname;
        private string username;
        private string password;
        private string indexUrl;

        public ElasticSearchContext(string hostname, string username, string password, string indexUrl)
        {
            this.hostname = hostname;
            this.username = username;
            this.password = password;
            this.indexUrl = indexUrl;
        }

        public RestClient CreateRestClientForElasticRequests()
        {
            UriBuilder builder = new UriBuilder("http://" + hostname + ":9200/" + indexUrl);

            Uri uri = builder.Uri;
            RestClient restClient = new RestClient(uri)
            {
                Authenticator =
                                 new HttpBasicAuthenticator(username, password)
            };
            return restClient;
        }
    }
}
