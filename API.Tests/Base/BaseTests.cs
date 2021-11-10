using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using API;
using Newtonsoft.Json;
using API.Resources;
using Data;
using Microsoft.Extensions.DependencyInjection;

namespace _15_API.Tests.Base
{
    public class BaseTests
    {
        protected readonly HttpClient TestClient;
        private string accessToken;

        protected BaseTests()
        {
            TestWebApplicationFactory<Startup> factory = new();
            TestClient = factory.CreateClient();
            TestClient.BaseAddress = new Uri("https://localhost:5001/api/");
        }

        protected async Task AuthenticateAs(int identityId)
        {
            if(TestClient.DefaultRequestHeaders.Contains("Authorization")) TestClient.DefaultRequestHeaders.Remove("Authorization");
            
            if(TestClient.DefaultRequestHeaders.Contains("IdentityId")) TestClient.DefaultRequestHeaders.Remove("IdentityId");

            TestClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await GetToken());
            TestClient.DefaultRequestHeaders.Add("IdentityId", identityId.ToString());
        }

        private async Task<string> GetToken()
        {
            if(accessToken != null)
            {
                return accessToken;
            }

            return await RenewAccessToken();
        }

        private async Task<string> RenewAccessToken()
        {
            accessToken = await GetAccessToken();
            return accessToken;
        }

        private async Task<string> GetAccessToken()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("client_id", "dex-api-client");
            dict.Add("client_secret", "Q!P5kqCukQBe77cVk5dqWHqx#8FaC2fDN&bstyxrHtw%5R@3Cz*Z");
            dict.Add("scope", "ProjectRead ProjectWrite UserRead UserWrite HighlightRead HighlightWrite");
            dict.Add("grant_type", "client_credentials");

            HttpRequestMessage request = new(HttpMethod.Post, $"https://localhost:5005/connect/token")
            {
                Content = new FormUrlEncodedContent(dict)
            };

            HttpClient client = new();
            HttpResponseMessage response = await client.SendAsync(request);
            string token = JsonConvert.DeserializeObject<AccessTokenReponse>(await response.Content.ReadAsStringAsync()).access_token;

            return token;
        }

        protected async Task<string> GetJwtAsync()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("client_id", "dex-api-client");
            dict.Add("client_secret", "Q!P5kqCukQBe77cVk5dqWHqx#8FaC2fDN&bstyxrHtw%5R@3Cz*Z");
            dict.Add("scope", "ProjectRead ProjectWrite UserRead UserWrite HighlightRead HighlightWrite");
            dict.Add("grant_type", "client_credentials");

            HttpRequestMessage request = new(HttpMethod.Post, "https://localhost:5005/connect/token")
            {
                Content = new FormUrlEncodedContent(dict)
            };

            HttpClient client = new();

            HttpResponseMessage response = await client.SendAsync(request);

            string token = JsonConvert.DeserializeObject<AccessTokenReponse>(await response.Content.ReadAsStringAsync()).access_token;

            return token;
        }
    }

    public class AccessTokenReponse
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
    }
}
