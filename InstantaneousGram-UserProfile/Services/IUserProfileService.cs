
using InstantaneousGram_UserProfile.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstantaneousGram_UserProfile.Services
{
    public interface IUserProfileService
    {
        Task<IEnumerable<UserProfile>> GetAllUserProfilesAsync();
        Task<UserProfile> GetUserProfileByIdAsync(int id);
        Task CreateUserProfileAsync(UserProfile userProfile);
        Task UpdateUserProfileAsync(UserProfile userProfile);
        Task DeleteUserProfileAsync(int id);
    }
}