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
            ProjectId = project.Id;
        }

        public PortfolioItem() { }

        public int Id { get; set; }

        [Required]
        public Portfolio Portfolio { get; set; }

        public int Position { get; set; }

        public int Type { get; set; }

        [Required]
        public string Content { get; set; }

        public int ProjectId { get; set; }
    }
}
