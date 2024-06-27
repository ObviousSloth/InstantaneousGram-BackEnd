using InstantaneousGram_UserProfile.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstantaneousGram_UserProfile.Services
{
    public interface IUserProfileService
    {
        Task<IEnumerable<UserProfile>> GetAllUserProfilesAsync();
        Task<UserProfile> GetUserProfileByAuthIdAsync(string authId);
        Task CreateUserProfileAsync(UserProfile userProfile);
        Task UpdateUserProfileAsync(UserProfile userProfile);
        Task DeleteUserProfileAsync(string authId);
    }
}
