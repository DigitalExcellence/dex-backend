using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API;
using Newtonsoft.Json;
using Data;
using Microsoft.Extensions.DependencyInjection;
using IdentityModel.Client;

namespace API.Tests.Base
{
    public class BaseTests
    {
        protected readonly HttpClient TestClient;
        protected readonly HttpClient AuthClient;
        private string accessToken;
        private string identityAddress = Environment.GetEnvironmentVariable("IdentityAddress");
        public string apiAddress = Environment.GetEnvironmentVariable("ApiAddress");

        protected BaseTests()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            TestWebApplicationFactory<Startup> factory = new TestWebApplicationFactory<Startup>();
            //TestClient = factory.CreateClient();
            AuthClient = new HttpClient(clientHandler);
            TestClient = new HttpClient(clientHandler);
            AuthClient.BaseAddress = new Uri(identityAddress);
            TestClient.BaseAddress = new Uri(apiAddress + "api/");
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
            accessToken = await GetJwtAsync();
            return accessToken;
        }

        //private async Task<string> GetAccessToken()
        //{
        //    Dictionary<string, string> dict = new Dictionary<string, string>();
        //    dict.Add("client_id", "dex-api-client");
        //    dict.Add("client_secret", "Q!P5kqCukQBe77cVk5dqWHqx#8FaC2fDN&bstyxrHtw%5R@3Cz*Z");
        //    dict.Add("scope", "ProjectRead ProjectWrite UserRead UserWrite HighlightRead HighlightWrite");
        //    dict.Add("grant_type", "client_credentials");

        //    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:5004/connect/token")
        //    {
        //        Content = new FormUrlEncodedContent(dict)
        //    };

        //    HttpClient client = new HttpClient();
        //    HttpResponseMessage response = await client.SendAsync(request);
        //    string token = JsonConvert.DeserializeObject<AccessTokenReponse>(await response.Content.ReadAsStringAsync()).Access_token;

        //    return token;
        //}

        protected async Task<string> GetJwtAsync()
        {
            //Dictionary<string, string> dict = new Dictionary<string, string>();
            //dict.Add("client_id", "dex-api-client");
            //dict.Add("client_secret", "Q!P5kqCukQBe77cVk5dqWHqx#8FaC2fDN&bstyxrHtw%5R@3Cz*Z");
            //dict.Add("scope", "ProjectRead ProjectWrite UserRead UserWrite HighlightRead HighlightWrite");
            //dict.Add("grant_type", "client_credentials");
            

            var disco = await TestClient.GetDiscoveryDocumentAsync(identityAddress);
            Console.WriteLine(identityAddress);
            Console.WriteLine(disco.HttpResponse);
            Console.WriteLine(disco.DeviceAuthorizationEndpoint);
            Console.WriteLine(disco.Issuer);

            var response = await TestClient.RequestTokenAsync(new TokenRequest
            {
                Address = disco.TokenEndpoint,
                GrantType = IdentityModel.OidcConstants.GrantTypes.ClientCredentials,
                ClientId = "Test",
                ClientSecret = "Test"
                //Parameters =
                //            {
                //                { "username", "bob"},
                //                { "password", "bob"},
                //                { "scope", "ProjectWrite"}
                //            }
            });
            Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            Console.WriteLine(response.HttpStatusCode);
            Console.WriteLine(response.AccessToken);
            //HttpClient client = new HttpClient(clientHandler);
            //HttpResponseMessage response = await client.SendAsync(request);
            //try
            //{
            //    string token = JsonConvert.DeserializeObject<AccessTokenReponse>(await response.Content.ReadAsStringAsync()).Access_token;

            //    return token;

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            return response.AccessToken;
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
