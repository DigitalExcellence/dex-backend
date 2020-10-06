using Models;

namespace API.Resources
{
    /// <summary>
    ///     The response result class of the UserProjectLike API request.
    /// </summary>
    public class UserProjectLikeResourceResult
    {
        /// <summary>
        ///     Gets or sets the identifier of the UserProjectLike.
        /// </summary>
        /// <value>
        ///     The identifier
        /// </value>
        public int Id { get; set; }
        /// <summary>
        ///     Gets or sets the project being liked.
        /// </summary>
        /// <value>
        ///     The Project class instance
        /// </value>
        public Project LikedProject { get; set; }
        /// <summary>
        ///     Gets or sets the user who created the project.
        /// </summary>
        /// <value>
        ///     The User class instance
        /// </value>
        public User CreatorOfProject { get; set; }
        /// <summary>
        ///     Gets or sets the id of the user who liked the project.
        /// </summary>
        /// <value>
        ///    The User identifier
        /// </value>
        public int UserId { get; set; }

    }

}
