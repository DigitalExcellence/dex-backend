using Newtonsoft.Json;
using NotificationSystem.Contracts;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSynchronizer
{
    public class DocumentUpdater : INotificationService
    {
        private ProjectES projectES;
        RestClient restClient;

        public DocumentUpdater()
        {
            restClient = new RestClient("http://localhost:9200/");
            restClient.Authenticator = new HttpBasicAuthenticator("elastic", "changeme");
        }

        public void ParsePayload(string jsonBody)
        {
            projectES = JsonConvert.DeserializeObject<ProjectES>(jsonBody);
        }


        public void ExecuteTask()
        {
            CreateOrUpdateDocument();
        }


        public bool ValidatePayload()
        {
            return true;
        }

        private void CreateOrUpdateDocument()
        {​​​​
            RestRequest request = new RestRequest("projectkeywords5/_doc/" + projectES.Id, Method.POST);
            request.AddParameter("application/json", JsonConvert.SerializeObject(projectES), ParameterType.RequestBody);
            IRestResponse response = restClient.Execute(request);
            if(response.IsSuccessful)
            {​​​​
                Console.WriteLine("Data successfully posted.");
            }​​​​
        }
    }
}











