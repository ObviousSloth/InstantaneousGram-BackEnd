using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using System.Net.Http;
using InstantaneousGram_UserProfile.Models;
using InstantaneousGram_UserProfile.Repositories;
using Microsoft.Extensions.Configuration;

namespace InstantaneousGram_UserProfile.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly HttpClient _httpClient;
        private readonly IConnection _rabbitMqConnection;
        private readonly IConfiguration _configuration;
        private readonly Auth0TokenService _auth0TokenService;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(IUserProfileRepository userProfileRepository, IConnection rabbitMqConnection, HttpClient httpClient, IConfiguration configuration, Auth0TokenService auth0TokenService, ILogger<UserProfileService> logger)
        {
            _userProfileRepository = userProfileRepository;
            _rabbitMqConnection = rabbitMqConnection;
            _httpClient = httpClient;
            _configuration = configuration;
            _auth0TokenService = auth0TokenService;
            _logger = logger;
        }

        public async Task<IEnumerable<UserProfile>> GetAllUserProfilesAsync()
        {
            return await _userProfileRepository.GetAllAsync();
        }

        public async Task<UserProfile> GetUserProfileByIdAsync(int id)
        {
            return await _userProfileRepository.GetByIdAsync(id);
        }
        public async Task<UserProfile> GetUserProfileByAuthIdAsync(string authId)
        {
            return await _userProfileRepository.GetByAuthIdAsync(authId);
        }

        public async Task CreateUserProfileAsync(UserProfile userProfile)
        {
            await _userProfileRepository.AddAsync(userProfile);
        }

        public async Task UpdateUserProfileAsync(UserProfile updatedUserProfile)
        {
            var existingUserProfile = await _userProfileRepository.GetByAuthIdAsync(updatedUserProfile.Auth0Id);
            if (existingUserProfile != null)
            {
                if (!string.IsNullOrEmpty(updatedUserProfile.Bio))
                    existingUserProfile.Bio = updatedUserProfile.Bio;

                if (!string.IsNullOrEmpty(updatedUserProfile.Username))
                    existingUserProfile.Username = updatedUserProfile.Username;

                if (!string.IsNullOrEmpty(updatedUserProfile.Email))
                    existingUserProfile.Email = updatedUserProfile.Email;

                await _userProfileRepository.UpdateAsync(existingUserProfile);

                if (!string.IsNullOrEmpty(updatedUserProfile.Username))
                {
                    await UpdateAuth0UserProfile(updatedUserProfile.Auth0Id, new { username = updatedUserProfile.Username });
                }

                if (!string.IsNullOrEmpty(updatedUserProfile.Email))
                {
                    await UpdateAuth0UserProfile(updatedUserProfile.Auth0Id, new { email = updatedUserProfile.Email });
                }
            }
        }

        private async Task UpdateAuth0UserProfile(string auth0Id, object updateData)
        {
            var token = await _auth0TokenService.GetManagementApiTokenAsync();
            var domain = _configuration["Auth0M2M:Domain"];

            var request = new HttpRequestMessage(HttpMethod.Patch, $"https://{domain}/api/v2/users/{auth0Id}");
            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Content = new StringContent(JsonSerializer.Serialize(updateData), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error updating Auth0 user: {StatusCode} - {ResponseBody}", response.StatusCode, responseBody);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task DeleteUserProfileAsync(int id)
        {
            var userProfile = await _userProfileRepository.GetByIdAsync(id);
            if (userProfile != null)
            {
                await _userProfileRepository.DeleteAsync(id);

                // Publish the deletion event
                using (var channel = _rabbitMqConnection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "user_deletion", type: ExchangeType.Fanout);

                    var message = JsonSerializer.Serialize(new { UserId = id });
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "user_deletion", routingKey: "", basicProperties: null, body: body);
                }
            }
        }
    }
}
