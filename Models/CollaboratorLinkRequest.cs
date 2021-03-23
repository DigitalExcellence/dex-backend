using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class CollaboratorLinkRequest
    {
        public int Id;

        [Required]
        public string RequestHash { get; set; }

        [Required]
        public Collaborator Collaborator { get; set; }
        
    }
}
