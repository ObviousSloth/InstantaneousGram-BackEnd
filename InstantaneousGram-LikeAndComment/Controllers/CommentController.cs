using InstantaneousGram_LikeAndComment.Data;
using InstantaneousGram_LikeAndComment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InstantaneousGram_LikeAndComment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly CommentRepository _commentRepository;

        public CommentsController(CommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] Comment comment)
        {
            await _commentRepository.AddCommentAsync(comment);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(string id, string userId)
        {
            var comment = await _commentRepository.GetCommentAsync(id, userId);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        // Additional actions for updating and deleting comments can be added here
    }
}
