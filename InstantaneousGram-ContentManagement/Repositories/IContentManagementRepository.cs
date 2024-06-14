using InstantaneousGram_ContentManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstantaneousGram_ContentManagement.Repositories
{
    public interface IContentManagementRepository
    {
        Task<IEnumerable<ContentManagement>> GetAllContentAsync();
        Task<ContentManagement> GetContentByIdAsync(int id);
        Task CreateContentAsync(ContentManagement content);
        Task UpdateContentAsync(ContentManagement content);
        Task DeleteContentAsync(int id);
        Task<IEnumerable<ContentManagement>> GetContentsByUserIdAsync(int userId); // New method
    }
}
