using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    public class SearchResultResource
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string Type { get; }
        public string Uri { get; }

        public SearchResultResource(int id, string name, string description, string type, string uri)
        {
            Id = id;
            Name = name;
            Description = description;
            Type = type;
            Uri = uri;
        }
    }
}
