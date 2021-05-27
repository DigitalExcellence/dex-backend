using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Settings
{
    public class HttpConnection
    {
        private string accessToken;
        public HttpClient Client { get; set; }
        public HttpClientFactory Factory { get; set; }

        public async Task<string> RenewAccessToken()
        {
            accessToken = await Factory.GetAccessToken();
            return accessToken;
        }

        public async Task<string> GetAccessToken()
        {
            if(accessToken != null) return accessToken;

            return await RenewAccessToken();
        }

        public async Task ApplyAuthenticationToClient()
        {
            await ApplyAuthenticationToClient("88421113");
        }

        public async Task ApplyAuthenticationToClient(string identityId)
        {
            string accessToken = await GetAccessToken();
            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            Client.DefaultRequestHeaders.Add("IdentityId", identityId);
        }
    }
}
