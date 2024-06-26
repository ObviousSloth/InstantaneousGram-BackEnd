using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InstantaneousGram_ImageAndVideoProcessing.Services;

namespace InstantaneousGram_ImageAndVideoProcessing.Controllers
{
    [Route("imageandvideoprocessing/api/[controller]")]
    [ApiController]
    public class ImageAndVideoController : ControllerBase
    {
        private readonly IImageAndVideoService _imageAndVideoService;

        public ImageAndVideoController(IImageAndVideoService imageAndVideoService)
        {
            _imageAndVideoService = imageAndVideoService;
        }

        [HttpPost("upload/image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string userId)
        {
            var result = await _imageAndVideoService.UploadImageAsync(file, userId);
            return Ok(result); // Return the full metadata object
        }

        [HttpPost("upload/video")]
        public async Task<IActionResult> UploadVideo([FromForm] IFormFile file, [FromForm] string userId)
        {
            var result = await _imageAndVideoService.UploadVideoAsync(file, userId);
            return Ok(result); // Return the full metadata object
        }


        [HttpGet("get/{mediaId}")]
        public async Task<IActionResult> GetMedia(string mediaId)
        {
            var media = await _imageAndVideoService.GetMediaAsync(mediaId);
            if (media == null)
            {
                return NotFound();
            }
            return Ok(media);
        }

        [HttpGet("metadata/{mediaId}")]
        public async Task<IActionResult> GetMetadata(string mediaId, [FromQuery] string userId)
        {
            var metadata = await _imageAndVideoService.GetMetadataAsync(mediaId, userId);
            if (metadata == null)
            {
                return NotFound();
            }
            return Ok(metadata);
        }

        [HttpGet("metadata/user/{userId}")]
        public async Task<IActionResult> GetMetadataByUserId(string userId)
        {
            var metadata = await _imageAndVideoService.GetMetadataByUserIdAsync(userId);
            return Ok(metadata);
        }

        [HttpDelete("metadata/{mediaId}")]
        public async Task<IActionResult> DeleteMetadata(string mediaId, [FromQuery] string userId)
        {
            await _imageAndVideoService.DeleteMetadataAsync(mediaId, userId);
            return NoContent();
        }
    }
}
