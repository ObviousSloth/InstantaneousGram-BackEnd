using InstantaneousGram_Authentication.Contracts;
using InstantaneousGram_Authentication.Models;
using InstantaneousGram_Authentication.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InstantaneousGram_Authentication.Controllers
{
    [ApiController]
    [Route("auth/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin login)
        {
            var token = await _authService.Authenticate(login.Email, login.Password);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            return Ok(new { Token = token });
        }

        [HttpGet("protected")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult Protected()
        {
            return Ok("You have accessed a protected endpoint!");
        }
    }
}
