using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API;
using Newtonsoft.Json;
using API.Resources;
using Data;
using Microsoft.Extensions.DependencyInjection;

namespace API.Tests.Base
{
    public class BaseTests
    {
        protected readonly HttpClient TestClient;
        private readonly HttpClientHandler clientHandler;
        private string accessToken;

        protected BaseTests()
        {
            clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            TestWebApplicationFactory<Startup> factory = new TestWebApplicationFactory<Startup>();
            TestClient = factory.CreateClient();
            TestClient.BaseAddress = new Uri("https://localhost:5001/api/");
        }

        protected async Task AuthenticateAs(int identityId)
        {
            if(TestClient.DefaultRequestHeaders.Contains("Authorization")) TestClient.DefaultRequestHeaders.Remove("Authorization");
            
            if(TestClient.DefaultRequestHeaders.Contains("IdentityId")) TestClient.DefaultRequestHeaders.Remove("IdentityId");

            string token = await GetToken();
            if(token != null)
            {
                TestClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            }
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

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:5005/connect/token")
            {
                Content = new FormUrlEncodedContent(dict)
            };

            HttpClient client = new HttpClient(clientHandler);
            HttpResponseMessage response = await client.SendAsync(request);
            try
            {
                string token = JsonConvert.DeserializeObject<AccessTokenReponse>(await response.Content.ReadAsStringAsync()).Access_token;

                return token;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }

    public class AccessTokenReponse
    {
        public string Access_token { get; set; }
        public string Expires_in { get; set; }
        public string Token_type { get; set; }
        public string Scope { get; set; }
    }
}
