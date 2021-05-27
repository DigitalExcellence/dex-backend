using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Settings
{
    public class HttpClientFactory
    {
        private HttpClientHandler handler { get; set; }

        public HttpClientFactory()
        {
            handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
        }

        public HttpClient CreateClient()
        {
            return CreateClient("5001");
        }

        public HttpClient CreateClient(string port)
        {
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri($"https://localhost:{port}");

            return client;
        }

        public async Task<string> GetAccessToken()
        {

            var dict = new Dictionary<string, string>();
            dict.Add("client_id", "dex-api-client");
            dict.Add("client_secret", "Q!P5kqCukQBe77cVk5dqWHqx#8FaC2fDN&bstyxrHtw%5R@3Cz*Z");
            dict.Add("scope", "ProjectRead ProjectWrite UserRead UserWrite HighlightRead HighlightWrite");
            dict.Add("grant_type", "client_credentials");

            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5004/connect/token")
            {
                Content = new FormUrlEncodedContent(dict)
            };

            var client = new HttpClient(handler);
            HttpResponseMessage response = await client.SendAsync(request);

            string responseContent = await response.Content.ReadAsStringAsync();

            AccessTokenReponse result = JsonConvert.DeserializeObject<AccessTokenReponse>(responseContent);

            string token = result.access_token;

            return token;
        }

        public HttpConnection GetConnection()
        {
            return new HttpConnection { Client = CreateClient(), Factory = this };
        }
    }
}
