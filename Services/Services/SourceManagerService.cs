using Search;
using Services.Sources;
using Sources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface ISourceManagerService
    {
        Task<IEnumerable<SearchResult>> SearchAsync(SearchRequest query);
    }

    public class SourceManagerService : ISourceManagerService
    {

        public SourceManagerService()
        {

        }

        public async Task<IEnumerable<SearchResult>> SearchAsync(SearchRequest request)
        {
            IList<SearchResult> results = new List<SearchResult>();
            foreach(SourceType sourceType in request.Sources)
            {
                ISource source = null;
                switch(sourceType) {
                    case SourceType.GITLAB:
                        source = new GitLabSource();
                        break;
                    case SourceType.GITHUB:
                        source = new GitHubSource();
                        break;
                }
                if(source != null)
                {
                    IEnumerable<SearchResult> searchResults = await source.Search((List<SearchQueryParameter>)request.QueryParameters);
                    foreach (SearchResult searchResult in searchResults)
                        results.Add(searchResult);
                }
            }
            return results;
        }
    }
}