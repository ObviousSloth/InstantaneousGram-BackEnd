using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InstantaneousGram_UserProfile.Services;
using InstantaneousGram_UserProfile.Models;
using InstantaneousGram_UserProfile.Managers;
using Microsoft.AspNetCore.Authorization;

namespace InstantaneousGram_UserProfile.Controllers
{
    [Route("userprofile/api/[controller]")]
    [ApiController]
    
    public class ProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly RabbitMQManager _rabbitMqManager;

        public ProfileController(IUserProfileService userProfileService, RabbitMQManager rabbitMqManager)
        {
            _userProfileService = userProfileService;
            _rabbitMqManager = rabbitMqManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserProfiles()
        {
            var userProfiles = await _userProfileService.GetAllUserProfilesAsync();
            return Ok(userProfiles);
        }

        [HttpGet("{authId}")]
        public async Task<IActionResult> GetUserProfile(string authId)
        {
            var userProfile = await _userProfileService.GetUserProfileByAuthIdAsync(authId);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }

        [HttpGet("auth/{authId}")]
        public async Task<IActionResult> GetUserProfileByAuthId(string authId)
        {
            var userProfile = await _userProfileService.GetUserProfileByAuthIdAsync(authId);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserProfile([FromBody] UserProfile userProfile)
        {
            if (userProfile == null)
            {
                return BadRequest(new { message = "Invalid input" });
            }
            await _userProfileService.CreateUserProfileAsync(userProfile);
            return CreatedAtAction(nameof(GetUserProfile), new { authId = userProfile.Auth0Id }, userProfile);
        }

        [HttpPut("{authId}")]
        public async Task<IActionResult> UpdateUserProfile(string authId, [FromBody] UserProfile userProfile)
        {
            if (authId != userProfile.Auth0Id)
            {
                return BadRequest("Auth0Id mismatch");
            }
            await _userProfileService.UpdateUserProfileAsync(userProfile);
            return NoContent();
        }

        [HttpPut("auth/{authId}")]
        public async Task<IActionResult> UpdateUserProfileByAuthId(string authId, [FromBody] UserProfile userProfile)
        {
            if (authId != userProfile.Auth0Id)
            {
                return BadRequest("Auth0Id mismatch");
            }
            await _userProfileService.UpdateUserProfileAsync(userProfile);
            return NoContent();
        }

        [HttpDelete("{authId}")]
        public async Task<IActionResult> DeleteUserProfile(string authId)
        {
            await _userProfileService.DeleteUserProfileAsync(authId);
            _rabbitMqManager.PublishUserDeletedEvent(authId);
            return NoContent();
        }
    }
}
