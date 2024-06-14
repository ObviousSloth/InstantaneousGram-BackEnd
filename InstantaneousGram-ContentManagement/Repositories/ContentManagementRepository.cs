using Instantaneousgram_ContentManagement.Data;
using InstantaneousGram_ContentManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstantaneousGram_ContentManagement.Repositories
{
    public class ContentManagementRepository : IContentManagementRepository
    {
        private readonly ApplicationDbContext _context;

        public ContentManagementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContentManagement>> GetAllContentAsync()
        {
            return await _context.ContentManagements.ToListAsync();
        }

        public async Task<ContentManagement> GetContentByIdAsync(int id)
        {
            return await _context.ContentManagements.FindAsync(id);
        }

        public async Task CreateContentAsync(ContentManagement content)
        {
            _context.ContentManagements.Add(content);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateContentAsync(ContentManagement content)
        {
            _context.ContentManagements.Update(content);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContentAsync(int id)
        {
            var content = await _context.ContentManagements.FindAsync(id);
            if (content != null)
            {
                _context.ContentManagements.Remove(content);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ContentManagement>> GetContentsByUserIdAsync(int userId) // New method
        {
            return await _context.ContentManagements.Where(c => c.UserID == userId).ToListAsync();
        }
    }
}
