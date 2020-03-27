using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Project
    {
        
        public int Id { get; set; }
        
        [Required]
        public User User { get; set; }
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        
        [Required]
        public string ShortDescription { get; set; }
        
        [Required]
        public string Uri { get; set; }
        
        public List<string> Contributors { get; set; }
        
        [Required]
        public DateTime Created { get; set; }
        
        [Required]
        public DateTime Updated { get; set; }

        public Project()
        {
            Contributors = new List<string>();
        }
    }
}