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
            AuthClient = new HttpClient(clientHandler);
            TestClient = new HttpClient(clientHandler);
            AuthClient.BaseAddress = new Uri(identityAddress);
            TestClient.BaseAddress = new Uri(apiAddress + "api/");
        }

        protected async Task AuthenticateAs(int identityId)
        {
            if(TestClient.DefaultRequestHeaders.Contains("Authorization")) TestClient.DefaultRequestHeaders.Remove("Authorization");
            
            if(TestClient.DefaultRequestHeaders.Contains("IdentityId")) TestClient.DefaultRequestHeaders.Remove("IdentityId");

            string token = await GetToken();

            if(token == null)
            {
                throw new Exception("NO JWT TOKEN!!!!!!!!!!!!!!!!!!!!!");
            }
            TestClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
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

        

        protected async Task<string> GetJwtAsync()
        {

            try
            {
                if(identityAddress == null)
                {
                    Console.WriteLine("identityadress null@1123!!!!@@@#$%^&");
                }
                TokenResponse response = await TestClient.RequestTokenAsync(new TokenRequest
                {
                    Address = identityAddress+"connect/token",
                    GrantType = IdentityModel.OidcConstants.GrantTypes.ClientCredentials,
                    ClientId = "Test",
                    ClientSecret = "Test",
                    Parameters =
                                {
                                    { "username", "bob"},
                                    { "password", "bob"},
                                    { "scope", "ProjectWrite"}
                                }
                });
                Console.WriteLine(response);
                Console.WriteLine(response.AccessToken);
                return response.AccessToken;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
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
