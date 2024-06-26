using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InstantaneousGram_UserProfile.Services
{
    public class Auth0TokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Auth0TokenService> _logger;

        public Auth0TokenService(HttpClient httpClient, IConfiguration configuration, ILogger<Auth0TokenService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GetManagementApiTokenAsync()
        {
            var clientId = _configuration["Auth0M2M:ClientId"];
            var clientSecret = _configuration["Auth0M2M:ClientSecret"];
            var audience = _configuration["Auth0M2M:ApiAudience"];
            var domain = _configuration["Auth0M2M:Domain"];

            var requestBody = new
            {
                client_id = clientId,
                client_secret = clientSecret,
                audience = audience,
                grant_type = "client_credentials"
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{domain}/oauth/token")
            {
                Content = requestContent
            };

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error getting Auth0 token: {StatusCode} - {ResponseContent}", response.StatusCode, responseContent);
                response.EnsureSuccessStatusCode();
            }

            var tokenResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
            return tokenResponse.GetProperty("access_token").GetString();
        }
    }
}
