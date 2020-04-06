using AutoMapper;
using Models;
using Search;
using Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface ISearchService
    {
        IEnumerable<SearchResult> SearchInternally(SearchRequest request);
        Task<IEnumerable<SearchResult>> SearchExternallyAsync(SearchRequest request);
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

        public IEnumerable<SearchResult> SearchInternally(SearchRequest request)
        {
            IEnumerable<Project> projects = _projectService.Search(request.QueryParameters.ToList().Select(queryparameter => queryparameter.value));

            IEnumerable<SearchResult> results = _mapper.Map<IEnumerable<Project>, IEnumerable<SearchResult>>(projects);

            return results;
        }
    }
}