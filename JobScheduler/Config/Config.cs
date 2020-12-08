using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;
using Serilog;
using System;

namespace JobScheduler
{

    public interface IConfig
    {
        Token GetJwtToken();

        string GetApiUrl();

    }

    public class Config : IConfig
    {
        public IdentityServerConfig IdentityServerConfig { get; set; }
        public ApiConfig ApiConfig { get; set; }

        public IConfiguration config { get; set; }

        public Config()
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            config = new ConfigurationBuilder()
                                           .AddJsonFile("appsettings.json", true, true)
                                           .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                                           .AddEnvironmentVariables()
                                           .Build();
            IdentityServerConfig  = config.GetSection("App")
                                          .GetSection("IdentityServerConfig")
                                          .Get<IdentityServerConfig>();
            ApiConfig = config.GetSection("App")
                              .GetSection("API")
                              .Get<ApiConfig>();

        }

        public Token GetJwtToken()
        {
            RestClient restClient = new RestClient(IdentityServerConfig.IdentityUrl);
            RestRequest restRequest = new RestRequest("connect/token") { Method = Method.POST };
            restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            restRequest.AddParameter("grant_type", "client_credentials");
            restClient.Authenticator = new HttpBasicAuthenticator(IdentityServerConfig.ClientId, IdentityServerConfig.ClientSecret);

            IRestResponse identityServerResponse = restClient.Execute(restRequest);

            if(!identityServerResponse.IsSuccessful)
            {

                Log.Logger.Error("Something went wrong: " + identityServerResponse.ErrorMessage);
            }
            Log.Logger.Information(identityServerResponse.Content);

            JObject json = JObject.Parse(identityServerResponse.Content);
            Token token = new Token(json["access_token"].ToString(), json["expires_in"].ToString(), json["token_type"].ToString(),json["scope"].ToString());

            return token;
        }


        public string GetApiUrl()
        {
            return ApiConfig.ApiUrl;
        }

    }


}
