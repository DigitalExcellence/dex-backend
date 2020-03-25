using Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sources
{
    public class GitLabSource : ISource
    {
        public Task<SearchResult> Search(SearchRequest request)
        {
            throw new NotImplementedException();
        }
    }
}