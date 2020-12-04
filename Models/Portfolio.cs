using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class Portfolio
    {
        public Portfolio()
        {
            PortfolioItem = new List<PortfolioItem>();
        }

        public int Id { get; set; }

        [Required]
        public User User { get; set; }

        public int Template { get; set; }

        public int? Icon { get; set; }

        public string Title { get; set; }

        public string PublicUri { get; set; }

        public List<PortfolioItem> PortfolioItem { get; set; }
    }
}
