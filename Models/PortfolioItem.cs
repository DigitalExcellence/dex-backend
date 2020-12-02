using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class PortfolioItem
    {
        public PortfolioItem(Portfolio portfolio, Project project)
        {
            Portfolio = portfolio;
            Project = project;
        }

        public PortfolioItem() { }

        public int Id { get; set; }

        [Required]
        public Portfolio Portfolio { get; set; }

        public int Position { get; set; }

        public int Type { get; set; }

        [Required]
        public string Content { get; set; }

        public Project Project { get; set; }
    }
}
