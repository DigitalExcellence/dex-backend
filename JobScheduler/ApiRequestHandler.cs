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
using System.Net;

namespace JobScheduler
{

    public interface IApiRequestHandler
    {
        List<UserTask> GetExpectedGraduationUsers();

        void SetGraduationTaskStatusToMailed(UserTask userTask);

    }

    public class ApiRequestHandler : IApiRequestHandler
    {

        private readonly RestClient apiClient;
        private string accessToken;
        private List<UserTask> userTasks;
        private IConfig config;

        public ApiRequestHandler(IConfig config)
        {
            apiClient = new RestClient(config.GetApiUrl());
            this.config = config;
        }


        public List<UserTask> GetExpectedGraduationUsers() {
        
            RestRequest restRequest = new RestRequest("api/UserTask/CreateUserTasks") { Method = Method.GET };

            if(accessToken == null)
            {
                
                dynamic data = JObject.Parse(config.GetJwtToken());
                accessToken = data.access_token;

                GetExpectedGraduationUsers();
            }
            else
            {
                restRequest.AddParameter("Authorization",
                                 string.Format("Bearer " + accessToken),
                                 ParameterType.HttpHeader);
                IRestResponse response = apiClient.Execute(restRequest);

                if(response.StatusCode.Equals(401))
                {
                    dynamic data = JObject.Parse(config.GetJwtToken());
                    accessToken = data.access_token;
                    GetExpectedGraduationUsers();
                }
                if(response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(response.ErrorMessage);
                }
                userTasks = JsonConvert.DeserializeObject<List<UserTask>>(response.Content);
            }
            return userTasks;
        }

        public void SetGraduationTaskStatusToMailed(UserTask userTask)
        {
            RestRequest restRequest = new RestRequest("api/UserTask/SetToMailed") { Method = Method.PUT };

            restRequest.AddParameter("Authorization",
                                     string.Format("Bearer " + accessToken),
                                     ParameterType.HttpHeader);
            restRequest.AddParameter("application/json", JsonConvert.SerializeObject(userTask), ParameterType.RequestBody);
            IRestResponse response = apiClient.Execute(restRequest);

            if(response.StatusCode.Equals(401))
            {
                    dynamic data = JObject.Parse(config.GetJwtToken());
                    accessToken = data.access_token;
                    SetGraduationTaskStatusToMailed(userTask);
            }
        }
    }
}
