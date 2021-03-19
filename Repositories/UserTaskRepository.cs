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
using Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{

    /// <summary>
    ///     This is the user task repository
    /// </summary>
    public interface IUserTaskRepository : IRepository<UserTask>
    {
        /// <summary>
        ///     This is the interface method which gets all user tasks for a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of user tasks</returns>
        public Task<List<UserTask>> GetUserTasksForUser(int userId);

        /// <summary>
        ///     This is the interface method which gets all user tasks
        /// </summary>
        /// <returns>List of user tasks</returns>
        Task<IEnumerable<UserTask>> GetAllUserTasks();

    }

    /// <summary>
    ///     This is the user task repository
    /// </summary>
    public class UserTaskRepository : Repository<UserTask>, IUserTaskRepository
    {
        /// <summary>
        ///     This is the constructor of the user task repository
        /// </summary>
        /// <param name="dbContext"></param>
        public UserTaskRepository(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        ///     This is the method which gets all user tasks for a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of user tasks</returns>
        public async Task<List<UserTask>> GetUserTasksForUser(int userId)
        {
            return await GetDbSet<UserTask>()
                         .Where(u => u.User.Id == userId)
                         .ToListAsync();
        }

        /// <summary>
        ///     This is the method which gets all user tasks
        /// </summary>
        /// <returns>List of user tasks</returns>
        public async Task<IEnumerable<UserTask>> GetAllUserTasks()
        {
            return await GetDbSet<UserTask>()
                         .Include(u => u.User)
                         .ToListAsync();
        }

    }

}
