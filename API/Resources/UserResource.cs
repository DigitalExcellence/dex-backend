namespace API.Resources
{
    public class UserResource
    {

        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public string IdentityId { get; set; }
        
        public LinkedServiceResource[] Services { get; set; }
        
    }
}