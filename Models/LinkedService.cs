using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [NotMapped]
    public class LinkedService
    {
        
        public ServiceType Service { get; set; }

        public string RefreshToken { get; set; }

    }
}