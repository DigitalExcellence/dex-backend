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

        void SetGraduationTaskStatusToMailed(int userTaskId);

    }

    public class ApiRequestHandler : IApiRequestHandler
    {

        private readonly RestClient apiClient;
        private Token _token;
        private List<UserTask> userTasks;
        private IConfig config;

        public ApiRequestHandler(IConfig config)
        {
            apiClient = new RestClient(config.GetApiUrl());
            this.config = config;
        }


        public List<UserTask> GetExpectedGraduationUsers() {
        
            RestRequest restRequest = new RestRequest("api/UserTask/CreateUserTasks") { Method = Method.GET };

            if(_token == null)
            {
                _token = config.GetJwtToken();

                GetExpectedGraduationUsers();
            }
            else
            {
                restRequest.AddParameter("Authorization",
                                 string.Format("Bearer " + _token.AccessToken),
                                 ParameterType.HttpHeader);
                IRestResponse response = apiClient.Execute(restRequest);

                if(response.StatusCode.Equals(401))
                {
                    _token = config.GetJwtToken();
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

        public void SetGraduationTaskStatusToMailed(int userTaskId)
        {
            RestRequest restRequest = new RestRequest("api/UserTask/SetToMailed/" + userTaskId) { Method = Method.PUT };

            restRequest.AddParameter("Authorization",
                                     string.Format("Bearer " + _token.AccessToken),
                                     ParameterType.HttpHeader);
            IRestResponse response = apiClient.Execute(restRequest);

            if(response.StatusCode.Equals(401))
            {
                    _token = config.GetJwtToken();
                    SetGraduationTaskStatusToMailed(userTaskId);
            }
            if(response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(response.ErrorMessage);
            }
        }
    }
}
