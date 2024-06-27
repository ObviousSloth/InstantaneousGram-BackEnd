using InstantaneousGram_UserProfile.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstantaneousGram_UserProfile.Repositories
{
    public interface IUserProfileRepository
    {
        Task<IEnumerable<UserProfile>> GetAllAsync();
        Task<UserProfile> GetByAuthIdAsync(string authId);
        Task AddAsync(UserProfile userProfile);
        Task UpdateAsync(UserProfile userProfile);
        Task DeleteAsync(string authId);
    }
}
