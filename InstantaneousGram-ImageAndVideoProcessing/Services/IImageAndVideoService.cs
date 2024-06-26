﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using InstantaneousGram_ImageAndVideoProcessing.Models;
using Microsoft.AspNetCore.Http;

namespace InstantaneousGram_ImageAndVideoProcessing.Services
{
    public interface IImageAndVideoService
    {
        Task<ImageOrVideoMetadata> UploadImageAsync(IFormFile file, string userId);
        Task<ImageOrVideoMetadata> UploadVideoAsync(IFormFile file, string userId);
        Task<GetResourceResult> GetMediaAsync(string publicId);
        Task<ImageOrVideoMetadata> GetMetadataAsync(string id, string userId);
        Task<IEnumerable<ImageOrVideoMetadata>> GetMetadataByUserIdAsync(string userId);
        Task DeleteMetadataAsync(string id, string userId);
        Task DeleteMediaByUserIdAsync(string userId);
    }
}
