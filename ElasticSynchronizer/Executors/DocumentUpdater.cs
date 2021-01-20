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

using ElasticSynchronizer.Configuration;
using ElasticSynchronizer.Models;
using Newtonsoft.Json;
using NotificationSystem.Contracts;
using RestSharp;
using System;

namespace ElasticSynchronizer.Executors
{
    public class DocumentUpdater : ICallbackService
    {
        private ESProjectDTO eSProject;
        private readonly RestClient restClient;
        private readonly Config config;

        public DocumentUpdater(Config config, RestClient restClient)
        {
            this.config = config;
            this.restClient = restClient;
        }

        /// <summary>
        /// Parses the payload.
        /// </summary>
        public void ParsePayload(string jsonBody)
        {
            eSProject = JsonConvert.DeserializeObject<ESProjectDTO>(jsonBody);
        }


        /// <summary>
        /// Executes the CreateOrUpdateDocument method.
        /// </summary>
        public void ExecuteTask()
        {
            CreateOrUpdateDocument();
        }


        /// <summary>
        /// Validates the payload.
        /// </summary>
        public bool ValidatePayload()
        {
            if(eSProject.Id <= 0)
            {
                throw new Exception("Invalid Project Id");
            }
            return true;
        }

        /// <summary>
        /// Sends API Call to the ElasticSearch Index, requesting the creation or update of given Project.
        /// </summary>
        private void CreateOrUpdateDocument()
        {
            string body = JsonConvert.SerializeObject(eSProject);
            RestRequest request = new RestRequest(config.Elastic.IndexUrl + "_doc/" + eSProject.Id, Method.PUT);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            Console.WriteLine(body);

            IRestResponse response = restClient.Execute(request);
            
            if(!response.IsSuccessful)
            {
                Console.WriteLine("Failed: " + response.StatusDescription + response.StatusCode);
                Console.WriteLine(response.Content);
            }
        }

        
    }
}











