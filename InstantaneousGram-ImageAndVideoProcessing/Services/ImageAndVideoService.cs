using System.Collections.Generic;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using InstantaneousGram_ImageAndVideoProcessing.Models;
using InstantaneousGram_ImageAndVideoProcessing.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InstantaneousGram_ImageAndVideoProcessing.Services
{
    public class ImageAndVideoService : IImageAndVideoService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IImageAndVideoRepository _imageAndVideoRepository;

        public ImageAndVideoService(ICloudinaryService cloudinaryService, IImageAndVideoRepository imageAndVideoRepository)
        {
            _cloudinaryService = cloudinaryService;
            _imageAndVideoRepository = imageAndVideoRepository;
        }

        public async Task<ImageOrVideoMetadata> UploadImageAsync(IFormFile file, string userId)
        {
            var uploadResult = await _cloudinaryService.UploadImageAsync(file);
            var createdAt = uploadResult.CreatedAt != default ? uploadResult.CreatedAt : DateTime.UtcNow;
            var metadata = new ImageOrVideoMetadata
            {
                Id = uploadResult.PublicId,
                UserId = userId,
                Url = uploadResult.SecureUrl.ToString(),
                Type = "image",
                Timestamp = createdAt
            };
            await _imageAndVideoRepository.AddMediaAsync(metadata);
            return metadata;
        }

        public async Task<ImageOrVideoMetadata> UploadVideoAsync(IFormFile file, string userId)
        {
            var uploadResult = await _cloudinaryService.UploadVideoAsync(file);
            var createdAt = uploadResult.CreatedAt != default ? uploadResult.CreatedAt : DateTime.UtcNow;
            var metadata = new ImageOrVideoMetadata
            {
                Id = uploadResult.PublicId,
                UserId = userId,
                Url = uploadResult.SecureUrl.ToString(),
                Type = "video",
                Timestamp = createdAt
            };
            await _imageAndVideoRepository.AddMediaAsync(metadata);
            return metadata;
        }


        public async Task<GetResourceResult> GetMediaAsync(string publicId)
        {
            return await _cloudinaryService.GetMediaAsync(publicId);
        }

        public async Task<ImageOrVideoMetadata> GetMetadataAsync(string id, string userId)
        {
            return await _imageAndVideoRepository.GetMediaByIdAsync(id, userId);
        }

        public async Task<IEnumerable<ImageOrVideoMetadata>> GetMetadataByUserIdAsync(string userId)
        {
            return await _imageAndVideoRepository.GetAllMediaByUserIdAsync(userId);
        }

        public async Task DeleteMetadataAsync(string id, string userId)
        {
            await _imageAndVideoRepository.DeleteMediaAsync(id, userId);
        }

        public async Task DeleteMediaByUserIdAsync(string userId)
        {
            var mediaList = await _imageAndVideoRepository.GetAllMediaByUserIdAsync(userId);

            foreach (var media in mediaList)
            {
                await _cloudinaryService.DeleteMediaAsync(media.Id);
                await _imageAndVideoRepository.DeleteMediaAsync(media.Id, userId);
            }
        }
    }
}
