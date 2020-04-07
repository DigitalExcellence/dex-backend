using System;
using System.Collections.Generic;
using System.Text;

namespace Search
{
    public class SearchQueryParameter
    {
        public SearchQueryParameterType Type { get; set; }
        public string Value { get; set;  }

        public SearchQueryParameter(SearchQueryParameterType type, string value)
        {
            this.Type = type;
            this.Value = value;
        }

        public SearchQueryParameter() { }
    }
}
