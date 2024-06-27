using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using InstantaneousGram_LikesAndComments.Models;
using InstantaneousGram_LikesAndComments.Services;

namespace InstantaneousGram_LikesAndComments.Controllers
{
    [ApiController]
    [Route("likesandcomments/api/[controller]")]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddLike([FromBody] Like like)
        {
            if (like == null || string.IsNullOrEmpty(like.UserId) || string.IsNullOrEmpty(like.PostId))
            {
                Console.WriteLine("Invalid like data received.");
                return BadRequest("Invalid like data.");
            }

            Console.WriteLine($"Received request to add like for user {like.UserId} on post {like.PostId}");
            await _likeService.AddLikeAsync(like);
            Console.WriteLine($"Like for user {like.UserId} on post {like.PostId} processed successfully.");
            return Ok();
        }



        [HttpDelete("{likeId}")]
        public async Task<IActionResult> DeleteLike(Guid likeId, [FromQuery] string userId)
        {
            await _likeService.DeleteLikeAsync(likeId, userId);
            return Ok();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetLikesByUserId(string userId)
        {
            var likes = await _likeService.GetLikesByUserIdAsync(userId);
            return Ok(likes);
        }

        [HttpGet("user/{userId}/post/{postId}")]
        public async Task<IActionResult> GetLikesByUserAndPost(string userId, string postId)
        {
            var like = await _likeService.GetLikeByUserAndPostAsync(userId, postId);
            return Ok(like);
        }
    }
}
