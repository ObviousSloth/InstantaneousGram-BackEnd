namespace InstantaneousGram_ApiGateway.Models
{
    public class Auth0
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; }    = string.Empty;
        public string ApiAudience { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
    }
}
