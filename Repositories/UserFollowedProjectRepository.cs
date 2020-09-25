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


    public class UserFollowedProjectRepository : Repository<UserFollowedProjectRepository>,IUserFollowedProjectRepository
    {
        public UserFollowedProjectRepository(DbContext dbContext) : base(dbContext) { }

        public void Add(UserFollowedProject followedProject)
        {
            //TODO: Specify DB set
            DbContext.Add(followedProject);
        }
    }
}
