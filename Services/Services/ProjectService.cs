using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Repositories;
using Services.Base;

namespace Services.Services
{
    public interface IProjectService : IService<Project>
    {
        Task<List<Project>> GetAllWithUsersAsync();

        Task<Project> FindWithUserAndCollaboratorsAsync(int id);
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

        public Task<Project> FindWithUserAndCollaboratorsAsync(int id)
        {
            return Repository.FindWithUserAndCollaboratorsAsync(id);
        }
    }
}