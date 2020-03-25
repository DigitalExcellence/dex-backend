using Sources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Search
{
    public class SearchRequest
    {
        public IEnumerable<SearchQueryParameter> QueryParameters { get; }
        public IEnumerable<SourceType> Sources { get; }

        public SearchRequest(IEnumerable<SearchQueryParameter> queryParameters, IEnumerable<SourceType> sources)
        {
            this.QueryParameters = queryParameters;
            this.Sources = sources;
        }
    }
}
