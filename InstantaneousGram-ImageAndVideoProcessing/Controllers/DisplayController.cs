using InstantaneousGram_ImageAndVideoProcessing.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InstantaneousGram_ImageAndVideoProcessing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisplayController : ControllerBase
    {
        private readonly ImageAndVideoRepository _repository;

        public DisplayController(ImageAndVideoRepository repository)
        {
            _repository = repository;
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

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllByUser(string userId)
        {
            var metadataList = await _repository.GetAllByUserAsync(userId);
            return Ok(metadataList);
        }
    }
}
