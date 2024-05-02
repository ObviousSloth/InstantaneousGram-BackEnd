namespace InstantaneousGram_UserProfile.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Auth0Id { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Profile_Picture { get; set; }
    }
}
