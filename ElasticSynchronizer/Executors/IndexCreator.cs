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
using SendGrid;
using System;
using System.Net.Http;
using System.Text;

namespace ElasticSynchronizer.Executors
{
    public class IndexCreator : ICallbackService
    {
        private string indexBody;
        private readonly RestClient restClient;
        private readonly HttpClient httpClient;
        private readonly Config config;

        public IndexCreator(Config config)
        {
            this.config = config;
            httpClient = new HttpClient();
            restClient = new RestClient(config.Elastic.Hostname)
                         {
                             Authenticator =
                                 new HttpBasicAuthenticator(config.Elastic.Username, config.Elastic.Password)
                         };
        }

        public void ParsePayload(string jsonBody)
        {
            indexBody = jsonBody;
        }


        public void ExecuteTask()
        {
            CreateIndex();
        }


        public bool ValidatePayload()
        {
            return true;
        }

        private async void CreateIndex()
        {
            Console.WriteLine("Hier: ");
            Console.WriteLine(config.Elastic.Hostname);
            Console.WriteLine(config.Elastic.IndexUrl);
            Console.WriteLine(indexBody);
            var request = await httpClient.PostAsync(config.Elastic.Hostname + config.Elastic.IndexUrl, new StringContent(indexBody, Encoding.UTF8, "application/json"));
            Console.WriteLine(request.StatusCode + " " + request.Content + " " + request.Content);
            if(!request.IsSuccessStatusCode)
            {
                Console.WriteLine(request.Content);
            }
        }

        
    }
}











