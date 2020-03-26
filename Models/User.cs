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
        
        public LinkedService[] Services { get; set; }
        
        public User()
        {
        }

        public User(int userId) : this()
        {
            Id = userId;
        }
        
    }
}