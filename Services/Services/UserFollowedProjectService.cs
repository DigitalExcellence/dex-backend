using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public interface IUserFollowedProjectService : IService<UserFollowedProject>
    {

       

    }

    public class UserFollowedProjectProjectService : Service<UserFollowedProjectRepository>
    {
        protected new IUserFollowedProjectRepository Repository => (IUserFollowedProjectRepository) base.Repository;

        public UserFollowedProjectProjectService(UserFollowedProjectRepository repository) : base(repository) { }

        private void Add(UserFollowedProject entity)
        {
            Repository.Add(entity);
        }

    }

}
