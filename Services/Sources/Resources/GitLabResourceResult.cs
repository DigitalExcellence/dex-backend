using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Sources.Resources
{
     
    /// <summary>
    /// Viewmodel for gitlab
    /// </summary>
    public class GitLabResourceResult
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string name { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string description { get; set; }
        /// <summary>
        /// Gets or sets the readme URL.
        /// </summary>
        /// <value>
        /// The readme URL.
        /// </value>
        public string readme_url { get; set; }
        /// <summary>
        /// Gets or sets the web URL.
        /// </summary>
        /// <value>
        /// The web URL.
        /// </value>
        public string web_url { get; set; }
    }
}
