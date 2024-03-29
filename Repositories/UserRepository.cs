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

using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json.Linq;
using Repositories.Base;
using Repositories.ElasticSearch;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Repositories
{

    /// <summary>
    ///     IUserRepository
    /// </summary>
    /// <seealso cref="User" />
    public interface IUserRepository : IRepository<User>
    {

        /// <summary>
        ///     Gets the user asynchronous by username.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The task that will get the User object.</returns>
        Task<User> GetUserAsync(int userId);

        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        ///     Gets the user by identity identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The task that will return the user object with that specified external identifier.</returns>
        Task<User> GetUserByIdentityIdAsync(string userId);

        /// <summary>
        ///     Removes the user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Returns if the user is removed</returns>
        Task<bool> RemoveUserAsync(int userId);

        /// <summary>
        ///     Users the has scope.
        /// </summary>
        /// <param name="identityId">The identity identifier.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>true if the user that has this scope.</returns>
        bool UserHasScope(string identityId, string scope);

        /// <summary>
        ///     Users the with role exists.
        /// </summary>
        /// <param name="role"></param>
        /// <returns>Returns true if a user has the given role, else false.</returns>
        bool UserWithRoleExists(Role role);

        /// <summary>
        ///     Gets the expected graduating user in time range between now and entered amount of months.
        /// </summary>
        /// <param name="amountOfMonths"></param>
        /// <returns></returns>
        Task<List<User>> GetAllExpectedGraduatingUsers(int amountOfMonths);

        List<int> GetSimilarUsers(int userId);
    }

    /// <summary>
    ///     UserRepository
    /// </summary>
    /// <seealso cref="User" />
    /// <seealso cref="IUserRepository" />
    public class UserRepository : Repository<User>, IUserRepository
    {
        private RestClient client;
        private Queries queries;
        /// <summary>
        ///     Initializes a new instance of the <see cref="UserRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public UserRepository(DbContext dbContext, IElasticSearchContext elasticSearchContext, Queries queries) : base(dbContext) {
            client = elasticSearchContext.CreateRestClientForElasticRequests();
            this.queries = queries;
        }

        /// <summary>
        ///     Finds the asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Returns the user object.</returns>
        public override async Task<User> FindAsync(int userId)
        {
            return await GetDbSet<User>()
                         .Where(s => s.Id == userId)
                         .Include(s => s.Role)
                         .ThenInclude(s => s.Scopes)
                         .Include(u => u.Institution)
                         .Include(f => f.UserProject)
                         .Include(s => s.LikedProjectsByUsers)
                         .SingleOrDefaultAsync();
        }

        

        public List<int> GetSimilarUsers(int userId)
        {
            List<int> similarUserIds = new List<int>();

            RestRequest request = new RestRequest("_search?size=0", Method.POST);
            string body = queries.SimilarUsers.Replace("ReplaceWithUserId", userId.ToString());
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse restResponse = client.Execute(request);

            if(restResponse.StatusCode != HttpStatusCode.OK)
            {
                return similarUserIds;
            }

            // Put user id's of similar users in a list.
            
            JToken users = JToken.Parse(restResponse.Content)
                              .SelectTokens("aggregations.user-liked.bucket.buckets").First();
            foreach(JToken user in users)
            {
                int id = user.First().ToObject<int>();
                similarUserIds.Add(id);
            }
            // Remove the user itself.
            if(similarUserIds.Count != 0)
            {
                similarUserIds.Remove(userId);
            }

            return similarUserIds;
        }


        /// <summary>
        ///     Gets the user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Returns the found user object.</returns>
        public async Task<User> GetUserAsync(int userId)
        {
            return await GetDbSet<User>()
                         .Where(s => s.Id == userId)
                         .Include(u => u.Role)
                         .ThenInclude(u => u.Scopes)
                         .Include(u => u.LikedProjectsByUsers)
                         .Include(u => u.Institution)
                         .Include(f => f.UserProject)
                         .SingleOrDefaultAsync();
        }

        /// <summary>
        ///     Gets the user by identity identifier asynchronous.
        /// </summary>
        /// <param name="identityId">The identity identifier.</param>
        /// <returns>Returns the found user object.</returns>
        public async Task<User> GetUserByIdentityIdAsync(string identityId)
        {
            return await GetDbSet<User>()
                         .Where(s => s.IdentityId == identityId)
                         .Include(u => u.Role)
                         .ThenInclude(u => u.Scopes)
                         .Include(u => u.Institution)
                         .Include(f => f.UserProject)
                         .Include(s => s.LikedProjectsByUsers)
                         .Include(u => u.UserTasks)
                         .FirstOrDefaultAsync();
        }

        /// <summary>
        ///     Removes the user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>if the user is removed.</returns>
        public async Task<bool> RemoveUserAsync(int userId)
        {
            User user = await GetDbSet<User>()
                              .Where(s => s.Id == userId)
                              .SingleOrDefaultAsync();

            if(user != null)
            {
                GetDbSet<User>()
                    .Remove(user);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Users the has scope.
        /// </summary>
        /// <param name="identityId">The identity identifier.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>true if a user has the given scope.</returns>
        public bool UserHasScope(string identityId, string scope)
        {
            User user = GetDbSet<User>()
                        .Where(s => s.IdentityId == identityId)
                        .Include(s => s.Role)
                        .ThenInclude(s => s.Scopes)
                        .FirstOrDefault();
            if(user?.Role == null)
            {
                return false;
            }
            foreach(RoleScope scp in user.Role.Scopes)
            {
                if(scp.Scope == scope)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Users the with role exists.
        /// </summary>
        /// <param name="role"></param>
        /// <returns>true if a user exists with the given role.</returns>
        public bool UserWithRoleExists(Role role)
        {
            return GetDbSet<User>()
                   .Include(s => s.Role)
                   .SingleOrDefault(r => r.Role.Id == role.Id) !=
                   null;
        }

        public async Task<List<User>> GetAllExpectedGraduatingUsers(int amountOfMonths)
        {
            DateTime now = DateTime.Now;
            DateTime max = DateTime.Now.AddMonths(amountOfMonths);


            return await GetDbSet<User>()
                         .Where(u => u.ExpectedGraduationDate >= now && u.ExpectedGraduationDate <= max)
                         .ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await GetDbSet<User>()
                .Where(user => user.Email == email).FirstOrDefaultAsync();
        }
    }

}
