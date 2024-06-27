using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet.Actions;

namespace InstantaneousGram_ImageAndVideoProcessing.Services
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);
        Task<VideoUploadResult> UploadVideoAsync(IFormFile file);
        Task<GetResourceResult> GetMediaAsync(string publicId);
        Task<DeletionResult> DeleteMediaAsync(string publicId);
    }
}
