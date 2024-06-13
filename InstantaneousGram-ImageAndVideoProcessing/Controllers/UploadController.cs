using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using InstantaneousGram_ImageAndVideoProcessing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InstantaneousGram_ImageAndVideoProcessing.Models.InstantaneousGram_ImageAndVideoProcessing.Models;
using InstantaneousGram_ImageAndVideoProcessing.Repositories;

namespace InstantaneousGram_ImageAndVideoProcessing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly ImageAndVideoRepository _repository;

        public UploadController(Cloudinary cloudinary, ImageAndVideoRepository repository)
        {
            _cloudinary = cloudinary;
            _repository = repository;
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadModel model)
        {
            if (model == null || model.Image == null || model.Image.Length == 0)
                return BadRequest("Invalid image");

            try
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(model.Image.FileName, model.Image.OpenReadStream())
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                var metadata = new ImageOrVideoMetadata
                {
                    Id = uploadResult.PublicId,
                    UserId = model.UserId,
                    Url = uploadResult.SecureUrl.ToString(),
                    Type = "image",
                    Timestamp = DateTime.UtcNow
                };

                await _repository.AddMetadataAsync(metadata);

                return Ok(new { metadata.Id, metadata.Url });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading image: {ex.Message}");
            }
        }

        [HttpPost("video")]
        public async Task<IActionResult> UploadVideo([FromForm] VideoUploadModel model)
        {
            if (model == null || model.Video == null || model.Video.Length == 0)
                return BadRequest("Invalid video");

            try
            {
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(model.Video.FileName, model.Video.OpenReadStream())
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                var metadata = new ImageOrVideoMetadata
                {
                    Id = uploadResult.PublicId,
                    UserId = model.UserId,
                    Url = uploadResult.SecureUrl.ToString(),
                    Type = "video",
                    Timestamp = DateTime.UtcNow
                };

                await _repository.AddMetadataAsync(metadata);

                return Ok(new { metadata.Id, metadata.Url });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading video: {ex.Message}");
            }
        }
    }
}
