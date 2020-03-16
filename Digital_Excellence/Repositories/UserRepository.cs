using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;

namespace Repositories
{
	public interface IUserRepository : IRepository<User>
	{
		Task<User> GetUserAsync(int userId);
		Task<bool> RemoveUserAsync(int userId);
	}

	public class UserRepository : Repository<User>, IUserRepository
	{
		public UserRepository(DbContext dbContext) : base(dbContext)
		{
		}

		public async Task<User> GetUserAsync(int userId)
		{
			return await GetDbSet<User>()
				.Where(s => s.Id == userId)
				.SingleOrDefaultAsync();
		}

		public async Task<bool> RemoveUserAsync(int userId)
		{
			User user = await GetDbSet<User>()
				.Where(s => s.Id == userId)
				.SingleOrDefaultAsync();

			if (user != null)
			{
				GetDbSet<User>().Remove(user);
				return true;
			}

			return false;
		}
	}
}