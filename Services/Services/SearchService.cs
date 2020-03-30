using Search;
using Sources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResult>> SearchInternallyAsync(SearchRequest request);
        Task<IEnumerable<SearchResult>> SearchExternallyAsync(SearchRequest request);

    }

    public class SearchService : ISearchService
    {
        private ISourceManagerService _sourceManagerService;
        private IProjectService _projectService;

        public SearchService(ISourceManagerService sourceManagerService, IProjectService projectService)
        {
            _sourceManagerService = sourceManagerService;
            _projectService = projectService;
        }

        public async Task<IEnumerable<SearchResult>> SearchExternallyAsync(SearchRequest request)
        {
            return await _sourceManagerService.SearchAsync(request);
        }

        public async Task<IEnumerable<SearchResult>> SearchInternallyAsync(SearchRequest request)
        {
            return await _projectService.SearchAsync(request);
        }
    }
}