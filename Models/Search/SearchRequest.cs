using Sources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Search
{
    public class SearchRequest
    {
        /// <summary>
        /// Query parameters to search with
        /// </summary>
        public IEnumerable<SearchQueryParameter> QueryParameters { get; }

        /// <summary>
        /// List of sources to search in
        /// </summary>
        public IEnumerable<ISource> Sources { get; }

        public SearchRequest(IEnumerable<SearchQueryParameter> queryParameters, IEnumerable<ISource> sources)
        {
            this.QueryParameters = queryParameters;
            this.Sources = sources;
        }
    }
}
