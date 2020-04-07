using Search;
using Sources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Sources
{
    public class GitHubSource : ISource
    {
        public Task<IEnumerable<SearchResult>> Search(List<SearchQueryParameter> queryParameters)
        {
            throw new NotImplementedException();
        }
    }
}