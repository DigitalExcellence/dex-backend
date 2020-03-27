using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Repositories;

namespace Services.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<Project>> SearchInternalProjects(string query);
        
        Task<IEnumerable<Project>> SearchInternalProjectsSkipTake(string query, int skip, int take);
        
        Task<int> SearchInternalProjectsCount(string query);
        
    }

    public class SearchService : ISearchService
    {
        private IProjectRepository _projectRepository;
        
        public SearchService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public virtual async Task<IEnumerable<Project>> SearchInternalProjects(string query)
        {
            return await _projectRepository.SearchAsync(query);
        }
        
        public virtual async Task<IEnumerable<Project>> SearchInternalProjectsSkipTake(string query, int skip, int take)
        {
            return await _projectRepository.SearchSkipTakeAsync(query, skip, take);
        }
        
        public virtual async Task<int> SearchInternalProjectsCount(string query)
        {
            return await _projectRepository.SearchCountAsync(query);
        }
        
    }
}