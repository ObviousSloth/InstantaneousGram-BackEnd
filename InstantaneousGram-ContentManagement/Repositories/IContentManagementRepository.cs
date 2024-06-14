using InstantaneousGram_ContentManagement.Models;

namespace InstantaneousGram_ContentManagement.Repositories
{
    public interface IContentManagementRepository
    {
        Task<IEnumerable<ContentManagement>> GetAllContentAsync();
        Task<ContentManagement> GetContentByIdAsync(int id);
        Task CreateContentAsync(ContentManagement content);
        Task UpdateContentAsync(ContentManagement content);
        Task DeleteContentAsync(int id);
    }
}
