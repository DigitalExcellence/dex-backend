using System;

namespace API.Resources
{
    public class ProjectResource
    {

        public int UserId { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string ShortDescription { get; set; }
        
        public string Uri { get; set; }
        
        public string[] Contributors { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Updated { get; set; }
    }
}