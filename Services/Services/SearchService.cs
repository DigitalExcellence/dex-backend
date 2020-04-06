using AutoMapper;
using Models;
using Repositories;
using Search;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResult>> SearchExternallyAsync(SearchRequest request);
        Task<IEnumerable<Project>> SearchInternalProjects(string query);
        
        Task<IEnumerable<Project>> SearchInternalProjectsSkipTake(string query, int skip, int take);
        
        Task<int> SearchInternalProjectsCount(string query);
        
    }

    public class SearchService : ISearchService
    {
        private ISourceManagerService _sourceManagerService;
        private IProjectService _projectService;
        private readonly IMapper _mapper;

        public SearchService(ISourceManagerService sourceManagerService, 
            IProjectService projectService, 
            IMapper mapper)
        {
            _sourceManagerService = sourceManagerService;
            _projectService = projectService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SearchResult>> SearchExternallyAsync(SearchRequest request)
        {
            return await _sourceManagerService.SearchAsync(request);
        }

        private IProjectRepository _projectRepository;

        public virtual async Task<IEnumerable<Project>> SearchInternalProjects(string query)
        {
            return await _projectRepository.SearchAsync(query);
        }
        
        // Search for projects
        // @param query The search query
        // @param skip The amount of results to skip
        // @param take The amount of results to return
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