/*
* Digital Excellence Copyright (C) 2020 Brend Smits
* 
* This program is free software: you can redistribute it and/or modify 
* it under the terms of the GNU Lesser General Public License as published 
* by the Free Software Foundation version 3 of the License.
* 
* This program is distributed in the hope that it will be useful, 
* but WITHOUT ANY WARRANTY; without even the implied warranty 
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
* See the GNU Lesser General Public License for more details.
* 
* You can find a copy of the GNU Lesser General Public License 
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JobScheduler
{

    /// <summary>
    ///     This is the interface of the API Request handler
    /// </summary>
    public interface IApiRequestHandler
    {

        Task<List<UserTask>> GetExpectedGraduationUsersAsync();

        void SetGraduationTaskStatusToMailed(int userTaskId);

    }

    /// <summary>
    ///     This is the implementation of the API request handler.
    /// </summary>
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

        /// <summary>
        ///     This is the method which requests the API for expecting graduating users
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserTask>> GetExpectedGraduationUsersAsync()
        {
            HttpResponseMessage response =
                await client.GetAsync("api/UserTask/CreateUserTasks/" + config.JobSchedulerConfig.TimeRange);
            return JsonConvert.DeserializeObject<List<UserTask>>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        ///     This is the method which sets the user tasks to status 'mailed'.
        /// </summary>
        /// <param name="userTaskId"></param>
        public void SetGraduationTaskStatusToMailed(int userTaskId)
        {
            client.PutAsync("api/UserTask/SetToMailed/" + userTaskId, null);
        }

    }

}
