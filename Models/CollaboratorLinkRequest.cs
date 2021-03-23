using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class CollaboratorLinkRequest
    {
        public int Id { get; set; }

        [Required]
        public string RequestHash { get; set; }

        [Required]
        public Collaborator Collaborator { get; set; }
        
    }
}
