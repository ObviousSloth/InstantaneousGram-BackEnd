using System;
using System.ComponentModel.DataAnnotations;

namespace InstantaneousGram_ContentManagement.Models
{
    public class ContentManagement
    {
        [Key]
        public int PostID { get; set; }

        [Required]
        public string Auth0Id { get; set; } // Changed UserID to Auth0Id

        [Required]
        public string MediaID { get; set; }

        public string Caption { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
