using System.Collections.Generic;
using System.Threading.Tasks;
using InstantaneousGram_ImageAndVideoProcessing.Models;

namespace InstantaneousGram_ImageAndVideoProcessing.Repositories
{
    public interface IImageAndVideoRepository
    {
        Task<IEnumerable<ImageOrVideoMetadata>> GetAllMediaByUserIdAsync(string userId);
        Task<ImageOrVideoMetadata> GetMediaByIdAsync(string mediaId, string userId);
        Task AddMediaAsync(ImageOrVideoMetadata media);
        Task UpdateMediaAsync(ImageOrVideoMetadata media);
        Task DeleteMediaAsync(string mediaId, string userId);
    }
}
