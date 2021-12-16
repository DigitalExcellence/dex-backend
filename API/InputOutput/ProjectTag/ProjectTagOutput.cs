namespace API.Resources
{

    /// <summary>
    ///     Object to return to frontend with the ProjectTag
    /// </summary>
    public class ProjectTagOutput
    {

        /// <summary>
        ///     Gets or sets the Id of the project tag.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the Name of the project tag.
        /// </summary>
        public string Name { get; set; }

    }

}
