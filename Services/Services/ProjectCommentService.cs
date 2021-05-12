using Models;
using Repositories;
using Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{

    public interface IProjectCommentService : IService<ProjectComment>
    {
        Task<List<ProjectComment>> GetProjectComments(int projectId);
    }

    public class ProjectCommentService : Service<ProjectComment>, IProjectCommentService
    {

        private new IProjectCommentRepository Repository => (IProjectCommentRepository) base.Repository;

        public ProjectCommentService(IProjectCommentRepository repository) : base(repository)
        {}

        public Task<List<ProjectComment>> GetProjectComments(int projectId)
        {
            return Repository.GetProjectComments(projectId);
        }
    }

}
