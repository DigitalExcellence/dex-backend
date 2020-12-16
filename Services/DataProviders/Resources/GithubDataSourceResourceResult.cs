using Newtonsoft.Json;

namespace Services.DataProviders.Resources
{
    /// <summary>
    /// Viewmodel for the project github datasource.
    /// </summary>
    public class GithubDataSourceResourceResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the incoming project.
        /// </summary>
        /// <value>
        /// The unique identifier of the project.
        /// </value>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the incoming project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the incoming project.
        /// </summary>
        /// <value>
        /// The unique description of the project.
        /// </value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the boolean that represents whether the incoming project is private.
        /// </summary>
        /// <value>
        /// <see langword="true"/> represents that the project is private, <see langword="false"/> represents that the project is public.
        /// </value>
        [JsonProperty("private")]
        public bool IsPrivate { get; set; }
    }
}
