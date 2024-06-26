using InstantaneousGram_UserProfile.Models;
using InstantaneousGram_UserProfile.Data;
using Microsoft.EntityFrameworkCore;

namespace InstantaneousGram_UserProfile.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly InstantaneousGramDbContext _context;

        public UserProfileRepository(InstantaneousGramDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserProfile>> GetAllAsync()
        {
            return await _context.UserProfiles.ToListAsync();
        }

        public async Task<UserProfile> GetByIdAsync(int id)
        {
            return await _context.UserProfiles.FindAsync(id);
        }
        public async Task<UserProfile> GetByAuthIdAsync(string authId)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(up => up.Auth0Id == authId);
        }
        public async Task AddAsync(UserProfile userProfile)
        {
            await _context.UserProfiles.AddAsync(userProfile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserProfile userProfile)
        {
            _context.UserProfiles.Update(userProfile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);
            if (userProfile != null)
            {
                _context.UserProfiles.Remove(userProfile);
                await _context.SaveChangesAsync();
            }
        }
    }
}
