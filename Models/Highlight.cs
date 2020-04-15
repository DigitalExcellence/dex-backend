using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Highlight
    {
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public Project Project { get; set; }

        [Required]
        public bool IsHighlighted { get; set; } = true;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
