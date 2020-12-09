using Newtonsoft.Json;
using NotificationSystem.Contracts;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSynchronizer
{
    public class DocumentDeleter : INotificationService
    {
        private ProjectES projectES;
        RestClient restClient;

        public DocumentDeleter()
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
            DeleteDocument();
        }


        public bool ValidatePayload()
        {
            return true;
        }

        private void DeleteDocument()
        {​​​​
            RestRequest request = new RestRequest("projectkeywords5/_doc/" + projectES.Id, Method.DELETE);
            IRestResponse response = restClient.Execute(request);
            if(response.IsSuccessful)
            {​​​​
                Console.WriteLine("Data successfully posted.");
            }​​​​
}
    }
}


