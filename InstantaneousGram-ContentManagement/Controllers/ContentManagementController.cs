using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InstantaneousGram_ContentManagement.Models;
using InstantaneousGram_ContentManagement.Services;

namespace Instantaneousgram_ContentManagement.Controllers
{
    [Route("contentmanagement/api/[controller]")]
    [ApiController]
    public class ContentManagementController : ControllerBase
    {
        private readonly IContentManagementService _contentService;

        public ContentManagementController(IContentManagementService contentService)
        {
            _contentService = contentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContent()
        {
            var content = await _contentService.GetAllContentAsync();
            return Ok(content);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContent(int id)
        {
            var content = await _contentService.GetContentByIdAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            return Ok(content);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContent([FromBody] ContentManagement content)
        {
            if (content == null)
            {
                return BadRequest(new { message = "Invalid input" });
            }
            await _contentService.CreateContentAsync(content);
            return CreatedAtAction(nameof(GetContent), new { id = content.PostID }, content);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContent(int id, [FromBody] ContentManagement content)
        {
            if (id != content.PostID)
            {
                return BadRequest();
            }
            await _contentService.UpdateContentAsync(content);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContent(int id)
        {
            await _contentService.DeleteContentAsync(id);
            return NoContent();
        }
    }
}
