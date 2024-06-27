using InstantaneousGram_UserProfile.Models;
using InstantaneousGram_UserProfile.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task DeleteAsync(string authId)
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.Auth0Id == authId);
            if (userProfile != null)
            {
                _context.UserProfiles.Remove(userProfile);
                await _context.SaveChangesAsync();
            }
        }
    }
}
