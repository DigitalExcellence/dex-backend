using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    /// <summary>
    /// The view model of a portfolio
    /// </summary>
    public class PortfolioResource
    {
        /// <summary>
        /// This sets or gets the template of the portfolio
        /// </summary>
        public int Template { get; set; }

        /// <summary>
        /// this sets or gets the portfolio title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// this sets or gets the portfolio Uri
        /// </summary>
        public string PublicUri { get; set; }
    }
}
