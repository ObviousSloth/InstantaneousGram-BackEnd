using InstantaneousGram_UserProfile.Models;
using InstantaneousGram_UserProfile.Repositories;

namespace InstantaneousGram_UserProfile.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
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
            await _userProfileRepository.DeleteAsync(id);
        }
    }
}
