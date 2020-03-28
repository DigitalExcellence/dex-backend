namespace API.Resources
{
    /// <summary>
    /// The view model of a user
    /// </summary>
    public class UserResource
    {
        /// <summary>
        /// This gets or sets the Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// This gets or sets the email
        /// </summary>
        public string Email { get; set; }
         /// <summary>
        /// This gets or sets the Identity Id from external identity provider
        /// </summary>
        public string IdentityId { get; set; }
        /// <summary>
        /// This gets or sets the linked services
        /// </summary>
        public LinkedServiceResource[] Services { get; set; }
        /// <summary>
        /// This gets or sets the ProfileUrl
        /// </summary>
        public string ProfileUrl { get; set; }
    }
}