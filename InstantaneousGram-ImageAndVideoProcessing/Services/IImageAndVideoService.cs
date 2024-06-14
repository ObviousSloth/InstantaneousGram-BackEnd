using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet.Actions;
using InstantaneousGram_ImageAndVideoProcessing.Models;

namespace InstantaneousGram_ImageAndVideoProcessing.Services
{
    public interface IImageAndVideoService
    {
        Task<string> UploadImageAsync(IFormFile file, string userId);
        Task<string> UploadVideoAsync(IFormFile file, string userId);
        Task<GetResourceResult> GetMediaAsync(string publicId);
        Task<ImageOrVideoMetadata> GetMetadataAsync(string id, string userId);
        Task<IEnumerable<ImageOrVideoMetadata>> GetMetadataByUserIdAsync(string userId);
        Task DeleteMetadataAsync(string id, string userId);
        Task DeleteMediaByUserIdAsync(string userId);
    }
}
