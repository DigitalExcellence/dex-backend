using Microsoft.VisualBasic;
using System;

namespace Models
{
    /// <summary>
    /// The Model class that represents
    /// individual projects liked by users.
    /// </summary>
    public class ProjectLike
    {

        public ProjectLike(Project likedProject, User projectLiker)
        {
            LikedProject = likedProject;
            ProjectLiker = projectLiker;
            Date = DateTime.Now;
        }

        public ProjectLike()
        {
        }

        /// <summary>
        /// Gets or set the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or set the individual project that
        /// liked by a user.
        /// </summary>
        /// <value>
        /// The Project instance.
        /// </value>
        public Project LikedProject { get; set; }

        /// <summary>
        /// Gets or set the project liker.
        /// </summary>
        /// <value>
        /// The User instance of the one who likes the project.
        /// </value>
        public User ProjectLiker { get; set; }

        /// <summary>
        /// Gets or sets the user who liked the project.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }

        public DateTime Date { get; set; }
    }

}
