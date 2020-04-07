using Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sources
{
    public interface ISource
    {
        Task<IEnumerable<SearchResult>> Search(List<SearchQueryParameter> queryParameters);
    }
}