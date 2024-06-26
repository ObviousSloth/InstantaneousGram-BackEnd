using InstantaneousGram_ContentManagement.Models;
using InstantaneousGram_ContentManagement.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstantaneousGram_ContentManagement.Services
{
    public class ContentManagementService : IContentManagementService
    {
        private readonly IContentManagementRepository _contentRepository;

        public ContentManagementService(IContentManagementRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public async Task<IEnumerable<ContentManagement>> GetAllContentAsync()
        {
            return await _contentRepository.GetAllContentAsync();
        }

        public async Task<ContentManagement> GetContentByIdAsync(int id)
        {
            return await _contentRepository.GetContentByIdAsync(id);
        }

        public async Task CreateContentAsync(ContentManagement content)
        {
            await _contentRepository.CreateContentAsync(content);
        }

        public async Task UpdateContentAsync(ContentManagement content)
        {
            await _contentRepository.UpdateContentAsync(content);
        }

/*        public async Task DeleteContentAsync(int id)
        {
            await _contentRepository.DeleteContentAsync(id);
        }

        public async Task DeleteAllContentByUserAsync(int userId)
        {
            var contents = await _contentRepository.GetContentsByUserIdAsync(userId);
            foreach (var content in contents)
            {
                await _contentRepository.DeleteContentAsync(content.PostID);
            }
        }*/
    }
}
