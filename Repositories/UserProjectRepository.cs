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

using Microsoft.EntityFrameworkCore;
using Models;
using Models.Defaults;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories
{

    public interface IUserProjectRepository : IRepository<UserProject>
    {

        Task<List<Project>> GetAllWithUsersAsync(
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null
            );

        Task<int> CountAsync(bool? highlighted = null);

        Task<IEnumerable<Project>> SearchAsync(
            string query,
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null
        );

        Task<int> SearchCountAsync(string query, bool? highlighted = null);
        void Remove(UserProject userProject);

        Task<Project> FindWithUserAndCollaboratorsAsync(int id);

        bool CheckIfUserFollows(int userId,int projectId);

    }

    public class UserProjectRepository : Repository<UserProject>, IUserProjectRepository
    {

        public UserProjectRepository(DbContext dbContext) : base(dbContext) { }

        public override void Add(UserProject entity)
        {
            DbContext.Add(entity);
        }

        public Task<int> CountAsync(bool? highlighted = null)
        {
            throw new NotImplementedException();
        }

        public Task<Project> FindWithUserAndCollaboratorsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetAllWithUsersAsync(int? skip = null, int? take = null, Expression<Func<Project, object>> orderBy = null, bool orderByAsc = true, bool? highlighted = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Project>> SearchAsync(string query, int? skip = null, int? take = null, Expression<Func<Project, object>> orderBy = null, bool orderByAsc = true, bool? highlighted = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> SearchCountAsync(string query, bool? highlighted = null)
        {
            throw new NotImplementedException();
        }

        bool IUserProjectRepository.CheckIfUserFollows(int userId, int projectId)
        {
            UserProject userProject = GetDbSet<UserProject>()
                              .Where(s => (s.UserId == userId) && s.Project.Id == projectId)
                              .SingleOrDefault();
            if(userProject != null)
            {
                return true;
            }
            return false;
        }

        void IUserProjectRepository.Remove(UserProject userProject)
        {
                GetDbSet<UserProject>()
                    .Remove(userProject);
        }
    }

}
