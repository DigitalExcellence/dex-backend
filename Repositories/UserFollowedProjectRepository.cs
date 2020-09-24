using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public interface IUserFollowedProjectRepository
    {
        void Add(UserFollowedProject entity);
    }


    public class UserFollowedProjectRepository : Repository<UserFollowedProjectRepository>
    {
        public UserFollowedProjectRepository(DbContext dbContext) : base(dbContext) { }
    }
}
