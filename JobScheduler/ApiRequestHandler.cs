using AutoMapper.Configuration;
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
        Task<List<UserTask>> GetExpectedGraduationUsersAsync();

        void SetGraduationTaskStatusToMailed(int userTaskId);

    }

    public class ApiRequestHandler : IApiRequestHandler
    {
        /// <summary>
        /// This is the HttpClient
        /// </summary>
        private readonly HttpClient client;

        public ApiRequestHandler(IHttpClientFactory factory)
        {
            client = factory.CreateClient("client");
        }

        public async Task<List<UserTask>> GetExpectedGraduationUsersAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/UserTask/CreateUserTasks/6");
            return JsonConvert.DeserializeObject<List<UserTask>>(await response.Content.ReadAsStringAsync());
        }

        public void SetGraduationTaskStatusToMailed(int userTaskId)
        {
            client.PutAsync("api/UserTask/SetToMailed/" + userTaskId, null);
        }

    }
}
