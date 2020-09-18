namespace Models
{

    public class UserProject
    {

        public int IdentityId { get; set; }
        public User User { get; set; }
        public int Id { get; set; }
        public Project Project { get; set; }

    }

}
