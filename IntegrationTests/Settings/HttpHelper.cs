using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Settings
{
    public class HttpHelper
    {
        public DefaultContractResolver ContractResolver { get; }

        public HttpHelper()
        {
            this.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
        }

        public StringContent GetHttpContent(object o)
        {
            return new StringContent(
                JsonConvert.SerializeObject(o, new JsonSerializerSettings
                {
                    ContractResolver = this.ContractResolver,
                    Formatting = Formatting.Indented
                }),
                Encoding.UTF8,
                "application/json"
                );
        }

        public async Task<dynamic> GetFromResponse(HttpResponseMessage response, Type type)
        {
            string responseStr = await response.Content.ReadAsStringAsync();
            dynamic responseObj = JsonConvert.DeserializeObject(responseStr, type);
            return responseObj;
        }
    }
}
