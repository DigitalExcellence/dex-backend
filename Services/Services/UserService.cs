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
using System.Reflection;
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

        Task<List<Project>> GetRecommendedProjects(int userId, int amountOfProjects);

    }

    public class UserService : Service<User>, IUserService
    {
        private readonly ElasticConfig elasticConfig;
        private readonly IProjectRepository projectRepository;

        public UserService(IUserRepository repository, IProjectRepository projectRepository, ElasticConfig config) : base(repository)
        {
            elasticConfig = config;
            this.projectRepository = projectRepository;
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
            User ownUserInfo = await Repository.FindAsync(ownUserId);
            User userRequestInfo = await Repository.FindAsync(requestUserId);
            return ownUserInfo.Institution == userRequestInfo.Institution && ownUserInfo.Institution != null;
        }

        public bool UserWithRoleExists(Role role)
        {
            return Repository.UserWithRoleExists(role);
        }

        public async Task<List<Project>> GetRecommendedProjects(int userId, int amountOfProjects)
        {
            // Get all similar users
            IEnumerable<int> similarUserIds = GetSimilarUsers(userId);
            List<Project> recommendedProjects = new List<Project>();

            // Iterate through users and their liked projects (which are not liked by user with userId) to find projects to recommend. When 10 projects are found the projects are returned.
            foreach(int user in similarUserIds)
            {
                List<Project> projectsFromUser = await projectRepository.GetLikedProjectsFromSimilarUser(userId, user);
                foreach(Project project in projectsFromUser.Where(project => !recommendedProjects.Contains(project)))
                {
                    recommendedProjects.Add(project);
                    if(recommendedProjects.Count == amountOfProjects)
                    {
                        return recommendedProjects;
                    }
                }
            }

            // If no recommendations were found for similar users then throw error. 
            if(recommendedProjects.Count == 0)
            {
                throw new RecommendationNotFoundException("similar user(s) do not have any projects you do not like yet.");
            }
            
            return recommendedProjects;
        }

        private IEnumerable<int> GetSimilarUsers(int userId)
        {
            List<int> foundUserIds = Repository.GetSimilarUsers(userId);

            // If no similar users were found then throw error.
            if(foundUserIds.Count == 0)
            {
                throw new RecommendationNotFoundException("no similar user(s) were found.");
            }

            return foundUserIds;
        }

       

    }
}
