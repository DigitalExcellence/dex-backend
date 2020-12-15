using Models;

namespace API.Resources
{

    /// <summary>
    ///     The response result class of the UserProjectLike API request.
    ///     The response of the API request will be carried-over to the caller
    ///     by using an instance of this class
    /// </summary>
    public class UserProjectLikeResourceResult
    {
        /// <summary>
        ///     Gets or sets the id of the project that being liked.
        /// </summary>
        /// <value>
        ///     The identifier of the liked Project
        /// </value>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the name of the project that being liked.
        /// </summary>
        /// <value>
        ///     The string representation of the Project's Name
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the short description of the project that being liked.
        /// </summary>
        /// <value>
        ///     The string representation of the Project's Short Description
        /// </value>
        public string ShortDescription { get; set; }

        /// <summary>
        ///     Gets or sets the uri of project that being liked.
        /// </summary>
        /// <value>
        ///    The string representation of the Project's Uri
        /// </value>
        public string Uri { get; set; }

        /// <summary>
        ///     Gets or sets the description of the project that being liked.
        /// </summary>
        /// <value>
        ///     The string representation of the Project's Description
        /// </value>
        public string Description { get; set; }

    }
}
