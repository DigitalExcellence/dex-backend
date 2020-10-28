using IdentityModel.Client;
using RestSharp;
using Services.Sources;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobScheduler
{
    public class Config
    {
        const string adress = "https://localhost:5005/";

        public string GetJwtToken()
        {
            string token = "";

            ClientCredentialsTokenRequest credentialsTokenRequest = new ClientCredentialsTokenRequest();
            credentialsTokenRequest.Address = adress;
            credentialsTokenRequest.ClientId = "dex-jobscheduler";
            credentialsTokenRequest.ClientSecret = "dex-jobscheduler";
            credentialsTokenRequest.Scope = "dex-api";



            var client = new RestClient(adress);

            //new from here

            RestRequest restRequest = new RestRequest("connect/token", Method.POST);
            restRequest.AddParameter("application/json", credentialsTokenRequest, ParameterType.RequestBody);
            restRequest.RequestFormat = DataFormat.Json;

            //Adding Json body as parameter to the post request

            IRestResponse restResponse = client.Execute(restRequest);

            return restResponse.Content;

        }
    }


}
