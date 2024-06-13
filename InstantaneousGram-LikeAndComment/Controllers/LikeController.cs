using InstantaneousGram_LikeAndComment.Data;
using InstantaneousGram_LikeAndComment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InstantaneousGram_LikeAndComment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly LikeRepository _likeRepository;

        public LikesController(LikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddLike([FromBody] Like like)
        {
            await _likeRepository.AddLikeAsync(like);
            return Ok();
        }

        [HttpGet("{id}/{userId}")]
        public async Task<IActionResult> GetLike(string id, string userId)
        {
            var like = await _likeRepository.GetLikeAsync(id, userId);
            if (like == null)
            {
                return NotFound();
            }
            return Ok(like);
        }

        // Additional actions for updating and deleting likes can be added here
    }
}
