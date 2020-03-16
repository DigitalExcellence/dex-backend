using System.Collections;
using System.Collections.Generic;
using Models;

namespace API.Resources
{
    /// <summary>
    /// The view model of a project
    /// </summary>
    public class ProjectResource
    {
        /// <summary>
        /// This gets or sets the userId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// This gets or sets the Title
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// This gets or sets the Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// This gets or sets the Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// This gets or sets the collaborators
        /// </summary>
        public ICollection<Collaborator> collaborators { get; set; }
    }
}