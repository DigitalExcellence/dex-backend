using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.Settings
{
    public class AccessTokenBody
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string scope { get; set; }
        public string grant_type { get; set; }
    }
}
