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

using Ganss.XSS;
using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Services
{

    public interface IUserProjectService : IService<UserProject>
    {
        void SaveFollowedProjectAsync(int userId, int projectId);


        /// <summary>
        /// Get a list of all the projects
        /// </summary>
        /// <param name="projectFilterParams">The parameters to filter, sort and paginate the projects</param>
        /// <returns>A list of all the projects</returns>
        Task<List<Project>> GetAllWithUsersAsync(ProjectFilterParams projectFilterParams);

        Task<Project> FindWithUserAndCollaboratorsAsync(int id);

        /// <summary>
        ///     Get the number of projects
        /// </summary>
        /// <param name="projectFilterParams">The parameters to filter, sort and paginate the projects</param>
        /// <returns>The number of projects</returns>
        Task<int> ProjectsCount(ProjectFilterParams projectFilterParams);

        /// <summary>
        ///     Get the total number of pages for the results
        /// </summary>
        /// <param name="projectFilterParams">The parameters to filter, sort and paginate the projects</param>
        /// <returns>The total number of pages for the results</returns>
        Task<int> GetProjectsTotalPages(ProjectFilterParams projectFilterParams);

        bool CheckIfUserFollows(int userId,int projectId);

    }

    public class UserProjectService : Service<UserProject>, IUserProjectService
    {

        public UserProjectService(IUserProjectRepository repository) : base(repository) { }

        protected new IUserProjectRepository Repository => (IUserProjectRepository) base.Repository;

        public void Add(UserProject entity)
        {
            Repository.Add(entity);
        }

        public Task AddAsync(UserProject entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<UserProject> entities)
        {
            throw new NotImplementedException();
        }

        public Task<Project> FindWithUserAndCollaboratorsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetAllWithUsersAsync(ProjectFilterParams projectFilterParams)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetProjectsTotalPages(ProjectFilterParams projectFilterParams)
        {
            throw new NotImplementedException();
        }

        public Task<int> ProjectsCount(ProjectFilterParams projectFilterParams)
        {
            throw new NotImplementedException();
        }

        public override void Remove(UserProject entity)
        {
            Repository.Remove(entity);
        }

        public void SaveFollowedProjectAsync(int userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public void Update(UserProject entity)
        {
            throw new NotImplementedException();
        }

        bool IUserProjectService.CheckIfUserFollows(int userId, int projectId)
        {
            if(Repository.CheckIfUserFollows(userId,projectId))
            {
                return true;
            }
            return false;
        }

        Task<UserProject> IService<UserProject>.FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<UserProject>> IService<UserProject>.GetAll()
        {
            throw new NotImplementedException();
        }
    }

}
