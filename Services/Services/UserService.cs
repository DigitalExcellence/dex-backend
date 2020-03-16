using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Repositories;
using Services.Base;

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
        protected new IUserRepository Repository => (IUserRepository) base.Repository;

        public UserService(IUserRepository repository) : base(repository)
        {
        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await Repository.GetUserAsync(userId);
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
