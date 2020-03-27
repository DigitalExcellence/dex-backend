using Models;
using Repositories;
using Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IProjectService : IService<Project>
    {
        Task<List<Project>> GetAllWithUsersAsync();
    }

    public class ProjectService : Service<Project>, IProjectService
    {
        protected new IProjectRepository Repository => (IProjectRepository)base.Repository;

        public ProjectService(IProjectRepository repository) : base(repository)
        {
        }

        public Task<List<Project>> GetAllWithUsersAsync()
        {
            return Repository.GetAllWithUsersAsync();
        }
    }
}