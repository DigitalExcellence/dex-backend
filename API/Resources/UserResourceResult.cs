namespace API.Resources
{
    /// <summary>
    /// the view model result of user.
    /// </summary>
    public class UserResourceResult
    {
        /// <summary>
        /// This gets or sets the Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// This gets or sets the Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// This gets or sets the Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// This gets or sets the Identity Id of an external provider
        /// </summary>
        public string IdentityId { get; set; }
        /// <summary>
        /// This gets or sets the ProfileUrl
        /// </summary>
        public string ProfileUrl { get; set; }
    }
}