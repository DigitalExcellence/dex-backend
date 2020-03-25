using System;
using System.Collections.Generic;
using System.Text;

namespace Search
{
    public class SearchQueryParameter
    {
        public SearchQueryParameterType type { get; }
        public string value { get; }

        public SearchQueryParameter(SearchQueryParameterType type, string value)
        {
            this.type = type;
            this.value = value;
        }
    }
}
