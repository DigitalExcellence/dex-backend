using Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sources
{
    public class GitHubSource : ISource
    {
        public Task<SearchResult> Search(IEnumerable<SearchQueryParameter> parameters)
        {
            throw new NotImplementedException();
        }
    }
}