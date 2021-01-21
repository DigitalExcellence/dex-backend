using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    /// <summary>
    /// the view model result of portfolio item
    /// </summary>
    public class PortfolioItemResource
    {
        /// <summary>
        /// This sets or gets the position
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// this sets or gets the portfolio type
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// this sets or gets the content
        /// </summary>
        public string Content { get; set; }


    }
}
