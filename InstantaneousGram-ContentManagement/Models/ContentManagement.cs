using System.ComponentModel.DataAnnotations;
using System;

namespace InstantaneousGram_ContentManagement.Models
{
    public class ContentManagement
    {
        [Key]
        public int PostID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int MediaID { get; set; }

        public string Caption { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
