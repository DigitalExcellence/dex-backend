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

        public SearchService(ISourceManagerService sourceManagerService)
        {
            _sourceManagerService = sourceManagerService;
        }

        public async Task<IEnumerable<SearchResult>> SearchExternallyAsync(SearchRequest request)
        {
            return await _sourceManagerService.SearchAsync(request);
        }

        public async Task<IEnumerable<SearchResult>> SearchInternallyAsync(SearchRequest request)
        {
            throw new NotImplementedException();
        }
    }
}