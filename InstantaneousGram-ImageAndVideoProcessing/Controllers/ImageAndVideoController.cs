using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using InstantaneousGram_ImageAndVideoProcessing.Models;
using InstantaneousGram_ImageAndVideoProcessing.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InstantaneousGram_ImageAndVideoProcessing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageAndVideoController : ControllerBase
    {
        private readonly ImageAndVideoRepository _repository;
        private readonly Cloudinary _cloudinary;

        public ImageAndVideoController(ImageAndVideoRepository repository, Cloudinary cloudinary)
        {
            _repository = repository;
            _cloudinary = cloudinary;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string userId, [FromForm] string type)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                PublicId = $"{userId}/{file.FileName}"
            };

            RawUploadResult uploadResult;
            if (type == "image")
            {
                uploadResult = await _cloudinary.UploadAsync(new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    PublicId = $"{userId}/{file.FileName}"
                });
            }
            else if (type == "video")
            {
                uploadResult = await _cloudinary.UploadAsync(new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    PublicId = $"{userId}/{file.FileName}"
                });
            }
            else
            {
                return BadRequest("Invalid type. Must be 'image' or 'video'.");
            }

            var metadata = new ImageOrVideoMetadata
            {
                Id = uploadResult.PublicId,
                UserId = userId,
                Url = uploadResult.SecureUrl.ToString(),
                Type = type,
                Timestamp = DateTime.UtcNow
            };

            await _repository.AddMetadataAsync(metadata);

            return Ok(new { metadata.Id, metadata.Url });
        }

        [HttpGet("{id}/{userId}")]
        public async Task<IActionResult> GetMetadata(string id, string userId)
        {
            var metadata = await _repository.GetMetadataAsync(id, userId);
            if (metadata == null)
            {
                return NotFound();
            }
            return Ok(metadata);
        }
    }
}
