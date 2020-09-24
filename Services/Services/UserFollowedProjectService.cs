using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IUserFollowedProjectService : IService<UserFollowedProject>
    {
        void SaveFollowedProjectAsync(int userId,int projectId);
    }

    public class UserFollowedProjectProjectService : Service<UserFollowedProjectRepository>,IUserFollowedProjectService
    {
        private readonly IUserRepository userRepository;
        private readonly IProjectRepository projectRepository;
        private readonly UserFollowedProjectRepository userFollowedProjectRepository;

        public UserFollowedProjectProjectService(IUserFollowedProjectRepository userFollowedRepository,IUserRepository userRepository,IProjectRepository projectRepository) : base(userFollowedRepository) {
            
            
        }
        protected new IProjectRepository Project => (IProjectRepository) base.Repository;
        protected new IUserFollowedProjectRepository UserFollowedProjectRepository => (IUserFollowedProjectRepository) base.Repository;
        protected new IUserRepository UserRepository => (IUserRepository) base.Repository;

        public  async void Add(UserFollowedProject userFollowed)
        {
            userFollowedProjectRepository.Add(userFollowed);
        }

        public Task AddAsync(UserFollowedProject entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<UserFollowedProject> entities)
        {
            throw new NotImplementedException();
        }

        public void Remove(UserFollowedProject entity)
        {
            throw new NotImplementedException();
        }

        public async void SaveFollowedProjectAsync(int userId,int projectId)
        {
            User user = await userRepository.FindAsync(userId);
            Project project = await projectRepository.FindAsync(projectId);
            UserFollowedProject followedProject = new UserFollowedProject(project,user);

            userFollowedProjectRepository.Add(followedProject);
        }

        public void Update(UserFollowedProject entity)
        {
            throw new NotImplementedException();
        }

        Task<UserFollowedProject> IService<UserFollowedProject>.FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<UserFollowedProject>> IService<UserFollowedProject>.GetAll()
        {
            throw new NotImplementedException();
        }
    }

    }


