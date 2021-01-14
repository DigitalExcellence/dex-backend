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

using MessageBrokerPublisher;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repositories;
using RestSharp;
using RestSharp.Authenticators;
using Services.Base;
using Services.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using File = Models.File;

namespace Services.Services
{
    public interface IUserService : IService<User>
    {
        Task<User> GetUserAsync(int userId);
        Task<User> GetUserByIdentityIdAsync(string identityId);

        Task<bool> RemoveUserAsync(int userId);

        User GetUserByUsername(string upn);

        bool UserHasScope(string identityId, string scope);

        Task<bool> HasSameInstitution(int ownUserId, int requestUserId);

        bool UserWithRoleExists(Role role);

        List<Project> GetRecommendedProjects(int userId);

    }

    public class UserService : Service<User>, IUserService
    {
        private RestClient elasticRestClient;
        private readonly ElasticConfig elasticConfig;

        public UserService(IUserRepository repository, ElasticConfig config) : base(repository)
        {
            elasticConfig = config;
            elasticRestClient = new RestClient(config.Hostname)
                                     {
                                         Authenticator = new HttpBasicAuthenticator(config.Username, config.Password)
                                     };
        }

        protected new IUserRepository Repository => (IUserRepository) base.Repository;

        public async Task<User> GetUserAsync(int userId)
        {
            return await Repository.GetUserAsync(userId).ConfigureAwait(false);
        }

        public async Task<User> GetUserByIdentityIdAsync(string identityId)
        {
            return await Repository.GetUserByIdentityIdAsync(identityId).ConfigureAwait(false);
        }

        public async Task<bool> RemoveUserAsync(int userId)
        {
            return await Repository.RemoveUserAsync(userId).ConfigureAwait(false);
        }

        public User GetUserByUsername(string upn)
        {
            throw new NotImplementedException();
        }

        public bool UserHasScope(string identityId, string scope)
        {
            return Repository.UserHasScope(identityId, scope);
        }

        public async Task<bool> HasSameInstitution(int ownUserId, int requestUserId)
        {
            var ownUserInfo = await Repository.FindAsync(ownUserId);
            var userRequestInfo = await Repository.FindAsync(requestUserId);
            return ownUserInfo.Institution == userRequestInfo.Institution && ownUserInfo.Institution != null;
        }

        public bool UserWithRoleExists(Role role)
        {
            return Repository.UserWithRoleExists(role);
        }

        public List<Project> GetRecommendedProjects(int userId)
        {
            int similarUserId = GetSimilarUser(userId);
            List<ESProjectFormat> elasticSearchProjects = GetLikedProjectsFromSimilarUser(userId, similarUserId);
            return ConvertProjects(elasticSearchProjects);
        }

        private int GetSimilarUser(int userId)
        {
            RestRequest request = new RestRequest(elasticConfig.IndexUrl + "_search?size=0", Method.POST);

            string body = System
                          .IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(),
                                                            "../Services/Resources/GetSimilarUsers.json"))
                          .Replace("ReplaceWithUserId", userId.ToString());

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse restResponse = elasticRestClient.Execute(request);
            if(!restResponse.IsSuccessful)
            {

            }

            int foundUserId = JToken.Parse(restResponse.Content)
                               .SelectTokens("aggregations.user-liked.bucket.buckets")
                               .First()[1].Value<int>("key");
            
            return foundUserId;
        }

        private List<ESProjectFormat> GetLikedProjectsFromSimilarUser(int userId, int similarUserId)
        {
            RestRequest request = new RestRequest(elasticConfig.IndexUrl + "_search", Method.POST);

            string body = System
                          .IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(),
                                                            "../Services/Resources/GetProjectRecommendations.json"))
                          .Replace("ReplaceWithUserId", userId.ToString())
                          .Replace("ReplaceWithSimilarUserId", similarUserId.ToString());

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse restResponse = elasticRestClient.Execute(request);
            if(!restResponse.IsSuccessful)
            {

            }

            /*List<ESProjectFormat> esProjects = JToken.Parse(restResponse.Content)
                                            .SelectTokens("hits.hits").Cast<List<ESProjectFormat>>();*/

            throw new NotImplementedException();
        }

        private List<Project> ConvertProjects(List<ESProjectFormat> elasticSearchProjects)
        {
            throw new NotImplementedException();
        }

    }
}
