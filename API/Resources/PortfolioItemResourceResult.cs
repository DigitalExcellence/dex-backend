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
    public class PortfolioItemResourceResult : PortfolioItemResource
    {
        /// <summary>
        /// this sets or gets the portfolio item id
        /// </summary>
        public int Id { get; set; }
    }
}
