using System;

namespace API.Resources
{
    /// <summary>
    ///     The view model result of UserProjectLike
    /// </summary>
    public class ProjectLikesResourceResult
    {
        /// <summary>
        ///     Gets or sets the id of the user who liked the project.
        /// </summary>
        /// <value>
        ///    The User identifier
        /// </value>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets the date of when the user has liked the project.
        /// </summary>
        public DateTime Date { get; set; }  
    }
}
