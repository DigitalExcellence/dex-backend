using System;
using System.Collections.Generic;

namespace API.Resources
{
    /// <summary>
    /// The view model result of project
    /// </summary>
    public class ProjectResourceResult
    {
        public int Id { get; set; }
        
        public UserResourceResult User { get; set; }

        /// <summary>
        /// This gets or sets the userId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// This gets or sets the Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// This gets or sets the Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// This gets or sets the Short Description
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// This gets or sets the Uri
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// This gets or sets the collaborators
        /// </summary>
        public ICollection<CollaboratorResourceResult> Collaborators { get; set; }
        /// <summary>
        /// This gets or sets the Created time of the project
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// This gets or sets the Updated time of the project
        /// </summary>
        public DateTime Updated { get; set; }
    }
}