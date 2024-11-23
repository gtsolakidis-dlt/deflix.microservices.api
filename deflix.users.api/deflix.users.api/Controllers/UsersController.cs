using deflix.users.api.DTOs;
using deflix.users.api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace deflix.users.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ApiController
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("signup")]
        public IActionResult Signup([FromBody] UserRegisterDto userDto)
        {
            var userId = _userService.Register(userDto);
            if (userId != Guid.Empty)
            {
                return Ok(new { message = "Registration successful" });
            }
            return BadRequest(new { message = "Registration failed. User already exists." });
        }

        [HttpPost("signin")]
        public IActionResult Signin([FromBody] UserLoginDto loginDto)
        {
            var user = _userService.Authenticate(loginDto.Username, loginDto.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            return Ok(new { message = "Signin successful", userId = user.UserId });
        }

        [HttpGet("profile/{userId}")]
        public IActionResult GetProfile(string userId)
        {
            if (!Guid.TryParse(userId, out var id))
            {
                return BadRequest(new { message = "Invalid UserId" });
            }

            var profile = _userService.GetUserProfile(id);
            if (profile == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(profile);
        }

        [HttpPost("profile/{userId}")]
        public IActionResult UpdateProfile(string userId, [FromBody] UserProfileUpdateDto profileUpdateDto)
        {
            if (!Guid.TryParse(userId, out var id))
            {
                return BadRequest(new { message = "Invalid UserId" });
            }

            if (_userService.UpdateUserProfile(id, profileUpdateDto))
            {
                return Ok(new { message = "Profile updated successfully" });
            }
            return NotFound(new { message = "User not found" });
        }

    }
}
