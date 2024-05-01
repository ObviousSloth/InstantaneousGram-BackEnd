using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InstantaneousGram_UserProfile.Data;
using InstantaneousGram_UserProfile.Models;

namespace InstantaneousGram_UserProfile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilesController : ControllerBase
    {
        private readonly InstantaneousGram_UserProfileContext _context;

        public UserProfilesController(InstantaneousGram_UserProfileContext context)
        {
            _context = context;
        }

        // GET: api/UserProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetUserProfile()
        {
          if (_context.UserProfile == null)
          {
              return NotFound();
          }
            return await _context.UserProfile.ToListAsync();
        }

        // GET: api/UserProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfile>> GetUserProfile(Guid id)
        {
          if (_context.UserProfile == null)
          {
              return NotFound();
          }
            var userProfile = await _context.UserProfile.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return userProfile;
        }

        // PUT: api/UserProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserProfile(Guid id, UserProfile userProfile)
        {
            if (id != userProfile.Id)
            {
                return BadRequest();
            }

            _context.Entry(userProfile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserProfiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserProfile>> PostUserProfile(UserProfile userProfile)
        {
          if (_context.UserProfile == null)
          {
              return Problem("Entity set 'InstantaneousGram_UserProfileContext.UserProfile'  is null.");
          }
            _context.UserProfile.Add(userProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserProfile", new { id = userProfile.Id }, userProfile);
        }

        // DELETE: api/UserProfiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProfile(Guid id)
        {
            if (_context.UserProfile == null)
            {
                return NotFound();
            }
            var userProfile = await _context.UserProfile.FindAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            _context.UserProfile.Remove(userProfile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserProfileExists(Guid id)
        {
            return (_context.UserProfile?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
