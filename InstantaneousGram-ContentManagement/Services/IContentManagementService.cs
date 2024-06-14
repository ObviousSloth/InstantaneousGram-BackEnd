using System.Collections.Generic;
using System.Threading.Tasks;
using InstantaneousGram_ContentManagement.Models;

namespace InstantaneousGram_ContentManagement.Services
{
    public interface IContentManagementService
    {
        Task<IEnumerable<ContentManagement>> GetAllContentAsync();
        Task<ContentManagement> GetContentByIdAsync(int id);
        Task CreateContentAsync(ContentManagement content);
        Task UpdateContentAsync(ContentManagement content);
        Task DeleteContentAsync(int id);
        Task DeleteAllContentByUserAsync(int userId); // Add this method
    }
}
