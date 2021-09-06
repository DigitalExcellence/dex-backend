namespace API.Resources
{

    /// <summary>
    ///     Object used to return to the frontend that shows a user task
    /// </summary>
    public class UserTaskResourceResult
    {

        /// <summary>
        ///     Gets or sets the id of the user task
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Get or sets the user
        /// </summary>
        public UserResourceResult UserResourceResult { get; set; }

        /// <summary>
        ///     Gets or sets the status of the user task
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        ///     Gets or sets the type of the user task
        /// </summary>
        public string Type { get; set; }

    }

}
