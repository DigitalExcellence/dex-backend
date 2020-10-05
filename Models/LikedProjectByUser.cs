namespace Models
{
    /// <summary>
    /// The Model class that represents
    /// individual projects liked by users.
    /// </summary>
    public class LikedProjectByUser
    {

        public LikedProjectByUser(Project likedProject, User creatorOfProject)
        {
            LikedProject = likedProject;
            CreatorOfProject = creatorOfProject;
        }

        public LikedProjectByUser()
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
        /// Gets or set the creator of the project.
        /// </summary>
        /// <value>
        /// The User instance.
        /// </value>
        public User CreatorOfProject { get; set; }

        /// <summary>
        /// Gets or sets the user who liked the project.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }
    }

}
