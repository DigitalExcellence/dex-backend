using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Highlight
    {
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }
        [Required]
        public Project Project { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
