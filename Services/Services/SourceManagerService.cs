using Search;
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

        public async Task<IEnumerable<SearchResult>> SearchAsync(SearchRequest query)
        {
            IList<SearchResult> results = new List<SearchResult>();
            foreach(ISource source in query.Sources)
            {
                IEnumerable<SearchResult> searchResults = await source.Search("temp");
                //SearchResult result = await source.Search(query.QueryParameters);
                foreach(SearchResult searchResult in searchResults)
                    results.Add(searchResult);
            }
            return results;
        }
    }
}