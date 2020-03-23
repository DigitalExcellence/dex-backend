namespace API.Resources
{
    public class ProjectResourceResult
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string ShortDescription { get; set; }
        
        public string Uri { get; set; }
        
        public string[] Contributors { get; set; }
        
    }
}