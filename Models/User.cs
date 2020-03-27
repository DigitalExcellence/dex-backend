using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string IdentityId { get; set; }

        public List<Project> Projects { get; set; }
        
        public List<LinkedService> Services { get; set; }
        
        public User()
        {
            Projects = new List<Project>();
            Services = new List<LinkedService>();
        }

        public User(int userId) : this()
        {
            Id = userId;
        }
        
    }
}