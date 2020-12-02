using Models;

namespace API.Resources
{
    /// <summary>
    ///     The view model result of UserProjectLike
    /// </summary>
    public class UserProjectLikeResource
    {
        /// <summary>
        ///     Gets or sets the Id of the resource result.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        ///     Gets or sets the project being liked of the resource result.
        /// </summary>
        /// <value>
        ///    The Project class instance
        /// </value>
        public Project LikedProject { get; set; }
        /// <summary>
        ///     Gets or sets the user who created the project
        /// </summary>
        /// <value>
        ///    The User class instance
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
