using Microsoft.AspNetCore.Identity;

namespace InstantaneousGram_Authentication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
