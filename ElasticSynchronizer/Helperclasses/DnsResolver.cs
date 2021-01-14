using ElasticSynchronizer.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ElasticSynchronizer.Helperclasses
{
    public class DnsResolver
    {
        private string hostName;

        public DnsResolver(Config config)
        {
            hostName = config.Elastic.Hostname;
        }

        public Uri GetUriElasticSearch()
        {
            string ip = Dns.GetHostEntry(hostName).AddressList.FirstOrDefault().ToString();
            UriBuilder builder = new UriBuilder(ip);
            return builder.Uri;
        }
    }
}
