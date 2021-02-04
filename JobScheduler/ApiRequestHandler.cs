using Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        ///     This is the HttpClient
        /// </summary>
        private readonly HttpClient client;

        /// <summary>
        ///     This is the configuration of the jobscheduler
        /// </summary>
        private readonly Config config;

        public ApiRequestHandler(IHttpClientFactory factory, Config config)
        {
            client = factory.CreateClient("client");
            this.config = config;
        }

        public async Task<List<UserTask>> GetExpectedGraduationUsersAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/UserTask/CreateUserTasks/" + config.JobSchedulerConfig.TimeRange);
            return JsonConvert.DeserializeObject<List<UserTask>>(await response.Content.ReadAsStringAsync());
        }

        public void SetGraduationTaskStatusToMailed(int userTaskId)
        {
            client.PutAsync("api/UserTask/SetToMailed/" + userTaskId, null);
        }

    }
}
