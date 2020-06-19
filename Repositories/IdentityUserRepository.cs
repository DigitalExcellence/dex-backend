using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{

    public interface IIdentityUserRepository: IRepository<IdentityUser>
    {

        Task<IdentityUser> FindAsync(string contextUserName);

    }

    public class IdentityUserRepository : Repository<IdentityUser>, IIdentityUserRepository
    {

        public IdentityUserRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<IdentityUser> FindAsync(string contextUserName)
        {
            throw new NotImplementedException();
        }

}
}
