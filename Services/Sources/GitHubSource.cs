using Search;
using Sources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Sources
{
    public class GitHubSource : ISource
    {
        public void getSource(string url)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SearchResult>> Search(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}