using System;
using System.Collections.Generic;
using System.Text;

namespace Search
{
    public class SearchResult
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }

        public SearchResult() { }

        public SearchResult(string name, string description, string type, string uri)
        {
            Name = name;
            Description = description;
            Type = type;
            Uri = uri;
        }
    }
}
