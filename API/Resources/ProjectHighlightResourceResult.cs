using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    /// <summary>
    ///     The view model result for a project that is returned when requesting highlights
    /// Only information that the highlight needs is returned
    /// </summary>
    public class ProjectHighlightResourceResult
    {

        /// <summary>
        ///     This gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     This gets or sets the Short Description
        /// </summary>
        public string ShortDescription { get; set; }
    }
}
