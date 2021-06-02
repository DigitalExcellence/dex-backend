using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    /// <summary>
    ///     The view model of a single autocompleted search project.
    /// </summary>
    public class AutocompleteProjectResourceResult
    {
        /// <summary>
        ///     Gets or sets the Id of the autocompleted search project.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        ///     Gets or sets the title of the autocompleted search project.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///     Gets or sets the Icon of the autocompleted search project.
        /// </summary>
        public File ProjectIcon { get; set; }
    }
}
