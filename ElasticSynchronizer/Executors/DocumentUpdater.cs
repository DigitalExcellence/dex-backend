using ElasticSynchronizer.Configuration;
using ElasticSynchronizer.Models;
using Newtonsoft.Json;
using NotificationSystem.Contracts;
using RestSharp;
using RestSharp.Authenticators;
using System;

namespace ElasticSynchronizer.Executors
{
    public class DocumentUpdater : INotificationService
    {
        private ProjectES projectEs;
        private readonly RestClient restClient;
        private readonly Config config;

        public DocumentUpdater(Config config)
        {
            this.config = config;
            restClient = new RestClient(config.Elastic.Hostname)
                         {
                             Authenticator =
                                 new HttpBasicAuthenticator(config.Elastic.Username, config.Elastic.Password)
                         };
        }

        public void ParsePayload(string jsonBody)
        {
            projectEs = JsonConvert.DeserializeObject<ProjectES>(jsonBody);
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
        {
            RestRequest request = new RestRequest(config.Elastic.IndexUrl + projectEs.Id, Method.POST);
            request.AddParameter("application/json", JsonConvert.SerializeObject(projectEs), ParameterType.RequestBody);
            IRestResponse response = restClient.Execute(request);
            if(!response.IsSuccessful)
            {
                Console.WriteLine(response);
            }
        }
    }
}











