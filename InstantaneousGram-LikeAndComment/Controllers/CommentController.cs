using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using InstantaneousGram_LikesAndComments.Models;
using InstantaneousGram_LikesAndComments.Services;

namespace InstantaneousGram_LikesAndComments.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] Comment comment)
        {
            await _commentService.AddCommentAsync(comment);
            return Ok();
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId, [FromQuery] string userId)
        {
            await _commentService.DeleteCommentAsync(commentId, userId);
            return Ok();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCommentsByUserId(string userId)
        {
            var comments = await _commentService.GetCommentsByUserIdAsync(userId);
            return Ok(comments);
        }

        [HttpGet("user/{userId}/post/{postId}")]
        public async Task<IActionResult> GetCommentsByUserAndPost(string userId, string postId)
        {
            var comments = await _commentService.GetCommentsByUserAndPostAsync(userId, postId);
            return Ok(comments);
        }
    }
}
