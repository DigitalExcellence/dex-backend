using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    /// <summary>
    /// The View model of a portfolioitem
    /// </summary>
    public class PortolioItemResourceResult
    {
        /// <summary>
        /// this sets or gets the portfolio item id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// this sets or gets the portfolioid
        /// </summary>
        public Portfolio PortfolioId { get; set; }

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

        /// <summary>
        /// this sets or gets the projectid
        /// </summary>
        public Project ProjectId { get; set; }
    }
}
