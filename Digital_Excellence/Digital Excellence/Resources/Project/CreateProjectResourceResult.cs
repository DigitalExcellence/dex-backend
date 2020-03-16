using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace API.Resources.Project
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateProjectResourceResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<ProjectMember> Contributors { get; set; }
    }
}
