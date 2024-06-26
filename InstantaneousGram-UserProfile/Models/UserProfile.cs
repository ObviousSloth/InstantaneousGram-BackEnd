using System;
using System.ComponentModel.DataAnnotations;

namespace InstantaneousGram_UserProfile.Models
{
    public class UserProfile
    {
        [Key]
        public string Auth0Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        public string ProfilePictureURL { get; set; }
        public string Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
