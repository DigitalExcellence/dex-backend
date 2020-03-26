using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Project
    {
        
        public int Id { get; set; }
        
        [Required]
        public User User { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        
        [Required]
        public string ShortDescription { get; set; }
        
        [Required]
        public string Uri { get; set; }
        
        public string[] Contributors { get; set; }
        
        [Required]
        public DateTime Created { get; set; }
        
        [Required]
        public DateTime Updated { get; set; }
    }
}