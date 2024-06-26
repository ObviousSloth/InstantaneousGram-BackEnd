using InstantaneousGram_UserProfile.Models;

namespace InstantaneousGram_UserProfile.Repositories
{
    public interface IUserProfileRepository
    {
        Task<IEnumerable<UserProfile>> GetAllAsync();
        Task<UserProfile> GetByIdAsync(int id);
        Task AddAsync(UserProfile userProfile);
        Task UpdateAsync(UserProfile userProfile);
        Task DeleteAsync(int id);
        Task<UserProfile> GetByAuthIdAsync(string authId);
    }
}
