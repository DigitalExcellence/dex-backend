using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    /// <summary>
    ///     The viewmodel that is returned by the Autocomplete search route in the Project controller.
    /// </summary>
    public class AutocompleteProjectsResource
    {
        /// <summary>
        ///     The List of Autocompleted search projects.
        /// </summary>
        public List<AutocompleteProjectResource> AutocompleteProjects { get; set; }
    }
}
