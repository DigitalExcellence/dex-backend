using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSynchronizer
{
    public class ProjectES
    {
            public DateTime Created { get; set; }
            public int Id { get; set; }
            public string ProjectName { get; set; }
            public string Description { get; set; }
            public List<int> Likes { get; set; }

        
    }
}
