namespace InstantaneousGram_UserProfile.Models
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string Auth0Id { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
