using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    /// <summary>
    /// viewmodel to return to frontend
    /// </summary>
    public class PortfolioItemResourceResult
    {

        /// <summary>
        /// this sets or gets the portfolio id
        /// </summary>
        public int PortfolioId { get; set; }

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
        /// this sets or gets the project
        /// </summary>
        public int ProjectId { get; set; }
    }
}
