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
using RestSharp.Authenticators;
using System;

namespace ElasticSynchronizer.Executors
{
    public class DocumentDeleter : INotificationService
    {
        private ProjectES projectEs;
        private readonly RestClient restClient;
        private readonly Config config;

        public DocumentDeleter(Config config)
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
            DeleteDocument();
        }


        public bool ValidatePayload()
        {
            return true;
        }

        private void DeleteDocument()
        {
            RestRequest request = new RestRequest(config.Elastic.IndexUrl + projectEs.Id, Method.DELETE);
            IRestResponse response = restClient.Execute(request);
            if(!response.IsSuccessful)
            {
                Console.WriteLine(response);
            }
        }

    }
}


