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
        public string Title { get; set; }
        /// <summary>
        /// This gets or sets the Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// This gets or sets the Url
        /// </summary>
        public string Url { get; set; }
    }
}
