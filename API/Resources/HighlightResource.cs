using System;

namespace API.Resources
{

    /// <summary>
    ///     The view model of a highlight
    /// </summary>
    public class HighlightResource
    {

        /// <summary>
        ///     This gets or sets the the id of the project that this highlight is associated with
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        ///     This gets or sets the start date that the highlight should start
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///     This gets or sets the end date that highlight should end
        /// </summary>
        public DateTime? EndDate { get; set; }

    }

}
