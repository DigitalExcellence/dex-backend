using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Models
{

    public class GithubDataSource : IDataSource
    {

        private readonly string clientSecret;
        private readonly string clientId;
        private readonly string baseUrl = "https://api.github.com/";

        public GithubDataSource(IConfiguration configuration)
        {
            clientId = configuration.GetSection($"{Name}ClientId")
                                    .Value;
            clientSecret = configuration.GetSection($"{Name}ClientSecret")
                                        .Value;
            OauthUrl = "https://github.com/login/oauth/authorize?client_id=" + clientId + $"&scope=repo&state={Name}";
        }

        public string Guid => "de38e528-1d6d-40e7-83b9-4334c51c19be";

        public string Name => "Github";

        public string OauthUrl { get; }

        public async Task<OauthTokens> GetTokens(string code)
        {
            using HttpClient client = new HttpClient();
            Dictionary<string, string> accessRefreshTokenRequirements = new Dictionary<string, string>
            {
                { "client_id",  clientId },
                { "client_secret", clientSecret},
                {"code", code },
                {"state", Name }
            };
            FormUrlEncodedContent data = new FormUrlEncodedContent(accessRefreshTokenRequirements);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Ruby77");
            HttpResponseMessage response = await client.PostAsync("https://github.com/login/oauth/access_token", data);

            if(response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<OauthTokens>();

            throw new Exception(response.ReasonPhrase);
        }

        public async Task<IEnumerable<Project>> GetAllProjects(string accessToken)
        {
            RestClient client = new RestClient(baseUrl + "user/repos");
            client.AddDefaultHeader("Authorization", $"Bearer {accessToken}");
            RestRequest request = new RestRequest(Method.GET);
            request.AddQueryParameter("visibility", "all");
            request.AddHeader("accept", "application/vnd.github.v3+json");

            IEnumerable<Project> response = (await client.ExecuteAsync<IEnumerable<Project>>(request)).Data;
            return response;
        }

    }

}
