using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InstantaneousGram_UserProfile.Data;
using InstantaneousGram_UserProfile.Models;
using InstantaneousGram_UserProfile.Services;

namespace InstantaneousGram_UserProfile.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserProfiles()
        {
            var userProfiles = await _userProfileService.GetAllUserProfilesAsync();
            return Ok(userProfiles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfile(int id)
        {
            var userProfile = await _userProfileService.GetUserProfileByIdAsync(id);
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
            return CreatedAtAction(nameof(GetUserProfile), new { id = userProfile.UserID }, userProfile);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserProfile(int id, [FromBody] UserProfile userProfile)
        {
            if (id != userProfile.UserID)
            {
                return BadRequest();
            }
            await _userProfileService.UpdateUserProfileAsync(userProfile);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProfile(int id)
        {
            await _userProfileService.DeleteUserProfileAsync(id);
            return NoContent();
        }
    }
}
