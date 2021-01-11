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
    public class LimitedPortfolioItemResourceResult
    {
        /// <summary>
        /// this sets or gets the portfolio id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// this sets or gets the project
        /// </summary>
        public int ProjectId { get; set; }
    }
}
