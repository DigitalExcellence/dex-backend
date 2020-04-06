using System;

namespace API.Resources
{
    public class SearchResultsResource
    {
        
        public SearchResultResource[] Results { get; set; }
        
        public string Query { get; set; }
        
        public int Count { get; set; }
        
        public int TotalCount { get; set; }
        
        public int? Page { get; set; }
        
        public int TotalPages { get; set; }
        
    }
}