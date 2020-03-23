namespace Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Email { get; set; }
        
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