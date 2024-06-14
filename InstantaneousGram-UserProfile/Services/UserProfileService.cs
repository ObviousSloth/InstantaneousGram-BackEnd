using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using InstantaneousGram_UserProfile.Models;
using InstantaneousGram_UserProfile.Repositories;

namespace InstantaneousGram_UserProfile.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IConnection _rabbitMqConnection;

        public UserProfileService(IUserProfileRepository userProfileRepository, IConnection rabbitMqConnection)
        {
            _userProfileRepository = userProfileRepository;
            _rabbitMqConnection = rabbitMqConnection;
        }

        public async Task<IEnumerable<UserProfile>> GetAllUserProfilesAsync()
        {
            return await _userProfileRepository.GetAllAsync();
        }

        public async Task<UserProfile> GetUserProfileByIdAsync(int id)
        {
            return await _userProfileRepository.GetByIdAsync(id);
        }

        public async Task CreateUserProfileAsync(UserProfile userProfile)
        {
            await _userProfileRepository.AddAsync(userProfile);
        }

        public async Task UpdateUserProfileAsync(UserProfile userProfile)
        {
            await _userProfileRepository.UpdateAsync(userProfile);
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
