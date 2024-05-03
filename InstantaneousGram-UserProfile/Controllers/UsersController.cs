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
    public class UsersController : ControllerBase
    {
        private readonly InstantaneousGram_UsersContextSQLite _context;

        public UsersController(InstantaneousGram_UsersContextSQLite context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    // GET: api/Users/auth/{authId}
        [HttpGet("auth/{authId}")]
        public async Task<ActionResult<User>> GetUserByAuthId(string authId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == authId);

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }
            catch (Exception ex)
            {

                Console.Error.WriteLine($"Error checking user: {ex}");
                return StatusCode(500, "Internal server error");
            }
       
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            try
            {
                var Dbuser = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == id);
                if (Dbuser == null)
                {
                    return NotFound();
                }

                // Ensure the Auth0Id is not modified
                if (Dbuser.Auth0Id != user.Auth0Id)
                {
                    return BadRequest();
                }

                // Update user properties
                Dbuser.Username = user.Username;
                Dbuser.Bio = user.Bio;
                Dbuser.Profile_Picture = user.Profile_Picture; // Include other properties as needed

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error updating user:", ex);
                return StatusCode(500); // Internal Server Error
            }
        }
        [HttpPut("Update/Bio/{authId}")]
        public async Task<IActionResult> PutUserBio(string authId, User user)
        {
            try
            {
                var Dbuser = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == authId);
                if (Dbuser == null)
                {
                    return NotFound();
                }

              

                // Update user properties (Bio and Username only)
                Dbuser.Bio = user.Bio;
                Dbuser.Username = user.Username;

                await _context.SaveChangesAsync();

                return Ok("Updated");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error updating user:", ex);
                return StatusCode(500); // Internal Server Error
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'InstantaneousGram_UsersContextSQLite.User'  is null.");
          }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool CheckUserExistsByAuthId(string authId) 
        {
            return (_context.Users?.Any(e => e.Auth0Id == authId)).GetValueOrDefault();
        }

        // GET: api/users/check/{auth0Id}
        [HttpGet("check/{auth0Id}")]
        public async Task<ActionResult<bool>> CheckUserExists(string auth0Id)
        {
            try
            {
                // Check if there is a user with the specified Auth0 ID
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);
                return user != null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error checking user: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
