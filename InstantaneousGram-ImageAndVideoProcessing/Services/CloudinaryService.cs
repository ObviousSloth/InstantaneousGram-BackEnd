using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace InstantaneousGram_ImageAndVideoProcessing.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };
            return await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task<VideoUploadResult> UploadVideoAsync(IFormFile file)
        {
            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };
            return await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task<GetResourceResult> GetMediaAsync(string publicId)
        {
            return await _cloudinary.GetResourceAsync(new GetResourceParams(publicId));
        }
        public async Task<DeletionResult> DeleteMediaAsync(string publicId) // Add this method
        {
            var deletionParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deletionParams);
        }
    }
}
