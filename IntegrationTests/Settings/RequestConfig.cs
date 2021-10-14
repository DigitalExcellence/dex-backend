using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.Settings
{
    public class RequestConfig
    {
        public HttpConnection Connection { get; set; }
        public HttpHelper HttpHelper { get; set; }
        public string Endpoint { get; set; }

    }
}
