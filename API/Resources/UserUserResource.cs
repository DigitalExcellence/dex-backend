using Models;

namespace API.Resources
{
    /// <summary>
    ///     The view model result of UserUser
    /// </summary>
    public class UserUserResource
    {
        /// <summary>
        ///     Get or Set Id of a useruser
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        ///     This gets or sets the user
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// user id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        ///     This gets or sets the followed user
        /// </summary>
        public User Followeduser { get; set; }
    }
}
