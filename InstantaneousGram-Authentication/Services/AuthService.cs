using InstantaneousGram_Authentication.Contracts;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace InstantaneousGram_Authentication.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthService(ITokenService tokenService, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<string> Authenticate(string email, string password)
        {
            // Authenticate with Auth0
            var clientId = _configuration["Auth0:ClientId"];
            var clientSecret = _configuration["Auth0:ClientSecret"];
            var domain = _configuration["Auth0:Domain"];
            var audience = _configuration["Auth0:Audience"];

            var client = new AuthenticationApiClient(new System.Uri($"https://{domain}/"));

            var request = new ResourceOwnerTokenRequest
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Realm = "Username-Password-Authentication", // The name of your connection
                Scope = "openid profile email",
                Audience = audience,
                Username = email,
                Password = password
            };

            var response = await client.GetTokenAsync(request);
            return response.AccessToken;
        }
    }
}
