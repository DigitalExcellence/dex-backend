using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    public class PortfolioResource
    {
        /// <summary>
        /// This sets or gets the user of the portfolio
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// This sets or gets the name of the portfolio
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This sets or gets the template of the portfolio
        /// </summary>
        public int Template { get; set; }

        /// <summary>
        /// This sets or gets the portfolio icon
        /// </summary>
        public int? Icon { get; set; }

        /// <summary>
        /// this sets or gets the portfolio title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// this sets or gets the portfoliouri
        /// </summary>
        public string PublicUri { get; set; }
    }
}
