using IdentityModel.Client;
using Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Net.Http;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobScheduler
{
    public class ApiRequestHandler
    {

        private readonly RestClient apiClient;
        private readonly string accessToken;

        public ApiRequestHandler(Uri baseUrlApi, string accessToken)
        {
           apiClient = new RestClient(baseUrlApi);
           this.accessToken = accessToken;
        }

        public string GetToken()
        {
            RestClient restClient = new RestClient("https://localhost:5005/");
            RestRequest restRequest = new RestRequest("connect/token") { Method = Method.POST };
            restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            restRequest.AddParameter("grant_type", "client_credentials");
            restClient.Authenticator = new HttpBasicAuthenticator("dex-jobscheduler", "dex-jobscheduler");

            IRestResponse identityServerResponse = restClient.Execute(restRequest);

            if (!identityServerResponse.IsSuccessful)
            {

                Log.Logger.Error("Something went wrong: " + identityServerResponse.ErrorMessage);
            }
            Log.Logger.Information(identityServerResponse.Content);
            return identityServerResponse.Content;
        }


            public List<User> GetExpectedGraduationUsers()
        {
            RestRequest restRequest = new RestRequest("/CallToAction");
            restRequest.AddParameter("Authorization",
                                 string.Format("Bearer " + accessToken),
                                 ParameterType.HttpHeader);
            IRestResponse response = apiClient.Execute(restRequest);
            if(!response.IsSuccessful)
            {
                // TODO: get new bearer token and try again.
                throw new NotImplementedException();
                
            }

            return JsonConvert.DeserializeObject<List<User>>(response.Content);
        }

    }
}
