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
using Newtonsoft.Json.Linq;

namespace JobScheduler
{
    public class ApiRequestHandler
    {

        private readonly RestClient apiClient;
        private string accessToken;
        private List<CallToAction> callToActions;

        public ApiRequestHandler(Uri baseUrlApi)
        {
            apiClient = new RestClient(baseUrlApi);
        }

        public string GetToken()
        {
            RestClient restClient = new RestClient("https://localhost:5005/");
            RestRequest restRequest = new RestRequest("connect/token") { Method = Method.POST };
            restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            restRequest.AddParameter("grant_type", "client_credentials");
            restClient.Authenticator = new HttpBasicAuthenticator("dex-jobscheduler", "dex-jobscheduler");

            IRestResponse identityServerResponse = restClient.Execute(restRequest);

            if(!identityServerResponse.IsSuccessful)
            {

                Log.Logger.Error("Something went wrong: " + identityServerResponse.ErrorMessage);
            }
            Log.Logger.Information(identityServerResponse.Content);
            return identityServerResponse.Content;
        }


        public List<CallToAction> GetExpectedGraduationUsers() {
        
            RestRequest restRequest = new RestRequest("api/CallToAction") { Method = Method.GET };

            if(accessToken == null)
            {
                
                dynamic data = JObject.Parse(GetToken());
                accessToken = data.access_token;

                GetExpectedGraduationUsers();
            }
            else
            {
                restRequest.AddParameter("Authorization",
                                 string.Format("Bearer " + accessToken),
                                 ParameterType.HttpHeader);
                IRestResponse response = apiClient.Execute(restRequest);

                if(!response.IsSuccessful)
                {
                    // TODO: maximum ammount of attempts to prevent endless trying again loop
                    accessToken = GetToken();
                    GetExpectedGraduationUsers();
                }
                callToActions = JsonConvert.DeserializeObject<List<CallToAction>>(response.Content);
            }
            return callToActions;
        }
    }
}
