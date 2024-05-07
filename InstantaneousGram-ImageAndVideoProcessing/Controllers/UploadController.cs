using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using InstantaneousGram_ImageAndVideoProcessing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InstantaneousGram_ImageAndVideoProcessing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;

        public UploadController(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
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

                return Ok(uploadResult);
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

                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading video: {ex.Message}");
            }
        }
    }
}
