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
using System;
using Models;
using Repositories;
using Services.Base;
using System.Threading.Tasks;

namespace Services.Services
{
	public interface IUserService : IService<User>
	{
		Task<User> GetUserAsync(int userId);
		Task<bool> RemoveUserAsync(int userId);
		User GetUserByUsername(string upn);
	}

    public class UserService : Service<User>, IUserService
    {
        protected new IUserRepository Repository => (IUserRepository)base.Repository;

        public UserService(IUserRepository repository) : base(repository)
        {
        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await Repository.GetUserAsync(userId);
        }

		public async Task<bool> RemoveUserAsync(int userId)
		{
			return await Repository.RemoveUserAsync(userId);
		}

		public User GetUserByUsername(string upn)
		{
			throw new NotImplementedException();
		}
	}
}
