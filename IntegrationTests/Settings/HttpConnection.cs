using System.Net.Http;
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

        public async Task ApplyAuthenticationToClient(string identityId)
        {

            string accessToken = await GetAccessToken();
            if(Client.DefaultRequestHeaders.Contains("Authorization"))
            {
                Client.DefaultRequestHeaders.Remove("Authorization");
            }
            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");


            if(Client.DefaultRequestHeaders.Contains("IdentityId"))
            {
                Client.DefaultRequestHeaders.Remove("IdentityId");
            }
            Client.DefaultRequestHeaders.Add("IdentityId", identityId);

        }
    }
}
