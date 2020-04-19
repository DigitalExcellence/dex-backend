using Models;

namespace API.Resources
{

    /// <summary>
    ///     The view model result of a highlight
    /// </summary>
    public class HighlightResourceResult : HighlightResource
    {

        /// <summary>
        ///     This gets or sets the the id of highlight
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     This gets or sets project associated with the highlight
        /// </summary>
        public Project Project { get; set; }

    }

}
