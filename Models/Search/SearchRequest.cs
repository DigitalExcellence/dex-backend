﻿using Sources;
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
        public IEnumerable<SearchQueryParameter> QueryParameters { get; set; }

        /// <summary>
        /// List of sources to search in
        /// </summary>
        public IEnumerable<ISource> Sources { get; set;  }

        public SearchRequest(IEnumerable<SearchQueryParameter> queryParameters, IEnumerable<ISource> sources)
        {
            this.QueryParameters = queryParameters;
            this.Sources = sources;
        }

        public SearchRequest()
        {
        }
    }
}
