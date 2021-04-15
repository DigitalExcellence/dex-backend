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

    /// <summary>
    ///     This is the interface of the user service
    /// </summary>
    public interface IUserService : IService<User>
    {

        /// <summary>
        ///     This is the interface method which gets a user by identifier asynchronous
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>User entity</returns>
        Task<User> GetUserAsync(int userId);

        /// <summary>
        ///     This is the interface method which gets the user by identity identifier asynchronous
        /// </summary>
        /// <param name="identityId"></param>
        /// <returns>User entity</returns>
        Task<User> GetUserByIdentityIdAsync(string identityId);

        /// <summary>
        ///     This is the interface method which removes a user by identifier asynchronous
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>boolean</returns>
        Task<bool> RemoveUserAsync(int userId);

        /// <summary>
        ///     This is the interface method which gets the user by username
        /// </summary>
        /// <param name="upn"></param>
        /// <returns>User entity</returns>
        User GetUserByUsername(string upn);

        /// <summary>
        ///     This is the interface method which checks if the user has a certain scope
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="scope"></param>
        /// <returns>boolean</returns>
        bool UserHasScope(string identityId, string scope);

        /// <summary>
        ///     This is the interface method which checks if the user has the same institution as another user
        /// </summary>
        /// <param name="ownUserId"></param>
        /// <param name="requestUserId"></param>
        /// <returns>boolean</returns>
        Task<bool> HasSameInstitution(int ownUserId, int requestUserId);

        /// <summary>
        ///     This is the interface method which checks if a user with a certain role exists
        /// </summary>
        /// <param name="role"></param>
        /// <returns>boolean</returns>
        bool UserWithRoleExists(Role role);

        /// <summary>
        ///     This is the interface method which get x amount of project recommendation for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="amountOfProjects"></param>
        /// <returns>List of projects</returns>
        Task<List<Project>> GetRecommendedProjects(int userId, int amountOfProjects);

        /// <summary>
        ///     This is the interface method which gets all expected graduating users.
        /// </summary>
        /// <param name="amountOfMonths"></param>
        /// <returns>List of users</returns>
        List<User> GetAllExpectedGraduatingUsers(int amountOfMonths);

    }

    /// <summary>
    ///     This is the user service
    /// </summary>
    public class UserService : Service<User>, IUserService
    {
        private readonly ElasticConfig elasticConfig;
        private readonly IProjectRepository projectRepository;

        /// <summary>
        ///     This is the constructor of the user service
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="projectRepository"></param>
        /// <param name="config"></param>
        public UserService(IUserRepository repository, IProjectRepository projectRepository, ElasticConfig config) : base(repository)
        {
            elasticConfig = config;
            this.projectRepository = projectRepository;
        }

        /// <summary>
        ///     Gets the repository
        /// </summary>
        protected new IUserRepository Repository => (IUserRepository) base.Repository;

        /// <summary>
        ///     This is the method which gets a user by identifier asynchronous
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(int userId)
        {
            return await Repository.GetUserAsync(userId)
                                   .ConfigureAwait(false);
        }

        /// <summary>
        ///     This is the method which gets the user by identity identifier asynchronous
        /// </summary>
        /// <param name="identityId"></param>
        /// <returns></returns>
        public async Task<User> GetUserByIdentityIdAsync(string identityId)
        {
            return await Repository.GetUserByIdentityIdAsync(identityId)
                                   .ConfigureAwait(false);
        }

        /// <summary>
        ///     This is the method which removes a user by identifier asynchronous
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>boolean</returns>
        public async Task<bool> RemoveUserAsync(int userId)
        {
            return await Repository.RemoveUserAsync(userId)
                                   .ConfigureAwait(false);
        }

        /// <summary>
        ///     This is the method which gets the user by username
        /// </summary>
        /// <param name="upn"></param>
        /// <returns>User entity</returns>
        public User GetUserByUsername(string upn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     This is the method which checks if the user has a certain scope
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="scope"></param>
        /// <returns>boolean</returns>
        public bool UserHasScope(string identityId, string scope)
        {
            return Repository.UserHasScope(identityId, scope);
        }

        /// <summary>
        ///     This is the method which checks if the user has the same institution as another user
        /// </summary>
        /// <param name="ownUserId"></param>
        /// <param name="requestUserId"></param>
        /// <returns>boolean</returns>
        public async Task<bool> HasSameInstitution(int ownUserId, int requestUserId)
        {
            User ownUserInfo = await Repository.FindAsync(ownUserId);
            User userRequestInfo = await Repository.FindAsync(requestUserId);
            return ownUserInfo.Institution == userRequestInfo.Institution && ownUserInfo.Institution != null;
        }

        /// <summary>
        ///     This is the method which checks if a user with a certain role exists
        /// </summary>
        /// <param name="role"></param>
        /// <returns>boolean</returns>
        public bool UserWithRoleExists(Role role)
        {
            return Repository.UserWithRoleExists(role);
        }

        /// <summary>
        ///     This is the method which get x amount of project recommendation for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="amountOfProjects"></param>
        /// <returns>List of projects</returns>
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
                throw new RecommendationNotFoundException("Similar user(s) do not have any projects you do not like yet.");
            }
            
            return recommendedProjects;
        }

        /// <summary>
        ///     This is the method which find a list of similar users (based on project likes) in order most similar to least similar user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of user ids</returns>
        private IEnumerable<int> GetSimilarUsers(int userId)
        {
            List<int> foundUserIds = Repository.GetSimilarUsers(userId);

            // If no similar users were found then throw error.
            if(foundUserIds.Count == 0)
            {
                throw new RecommendationNotFoundException("No similar user(s) were found.");
            }

            return foundUserIds;
        }

        /// <summary>
        ///     This is the interface method which gets all expected graduating users.
        /// </summary>
        /// <param name="amountOfMonths"></param>
        /// <returns>List of users</returns>
        public List<User> GetAllExpectedGraduatingUsers(int amountOfMonths)
        {
            List<User> users = Repository.GetAllExpectedGraduatingUsers(amountOfMonths)
                                         .Result;
            return users;
        }

    }

}
