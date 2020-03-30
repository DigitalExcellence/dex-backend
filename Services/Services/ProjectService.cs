using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Repositories;
using Search;
using Services.Base;

namespace Services.Services
{
    public interface IProjectService : IService<Project>
    {
        Task<IEnumerable<SearchResult>> SearchAsync(SearchRequest query);
    }

    public class ProjectService : Service<Project>, IProjectService
    {
        protected new IProjectRepository Repository => (IProjectRepository) base.Repository;

        public ProjectService(IProjectRepository repository) : base(repository)
        {

        }

        public async Task<IEnumerable<SearchResult>> SearchAsync(SearchRequest query)
        {
            throw new NotImplementedException();
        }
    }
}