using Search;
using Sources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
                SearchResult result = await source.Search(query);
                results.Add(result);
            }

            return results;
        }
    }
}