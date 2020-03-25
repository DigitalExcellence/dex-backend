using System;
using System.Collections.Generic;
using System.Text;

namespace Search
{
    public class SearchResult
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string Type { get; }
        public string Uri { get; }

        public SearchResult(int id, string name, string description, string type, string uri)
        {
            Id = id;
            Name = name;
            Description = description;
            Type = type;
            Uri = uri;
        }
    }
}
