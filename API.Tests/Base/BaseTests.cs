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
using API.Tests.Enums;

namespace API.Tests.Base
{
    public class BaseTests
    {
        protected readonly HttpClient TestClient;
        protected readonly HttpClient AuthClient;

        private string accessToken;
        private readonly string identityAddress = Environment.GetEnvironmentVariable("IdentityAddress");
        private readonly string apiAddress = Environment.GetEnvironmentVariable("ApiAddress");

        protected BaseTests()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            AuthClient = new HttpClient(clientHandler);
            TestClient = new HttpClient(clientHandler);

            AuthClient.BaseAddress = new Uri(identityAddress);
            TestClient.BaseAddress = new Uri(apiAddress + "api/");
        }

        protected async Task AuthenticateAs(UserRole identityId)
        {
            if(TestClient.DefaultRequestHeaders.Contains("Authorization")) TestClient.DefaultRequestHeaders.Remove("Authorization");
            
            if(TestClient.DefaultRequestHeaders.Contains("IdentityId")) TestClient.DefaultRequestHeaders.Remove("IdentityId");

            string token = await GetToken();

            if(token == null)
            {
                throw new Exception("NO JWT TOKEN!!!!!!!!");
            }

            TestClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            int id = (int)identityId;
            TestClient.DefaultRequestHeaders.Add("IdentityId", id.ToString());
        }

        private async Task<string> GetToken()
        {
            if(accessToken != null)
            {
                return accessToken;
            }

            accessToken = await GetJwtAsync();
            return accessToken;
        }

        protected async Task<string> GetJwtAsync()
        {
            try
            {
                if(identityAddress == null)
                {
                    Console.WriteLine("IdentityAddress is null!!!!!!!!");
                }

                TokenResponse response = await TestClient.RequestTokenAsync(new TokenRequest
                {
                    Address = identityAddress + "connect/token",
                    GrantType = IdentityModel.OidcConstants.GrantTypes.ClientCredentials,
                    ClientId = "XUnitIntegrationTests",
                    ClientSecret = "XUnitIntegrationTests"
                });

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
}
