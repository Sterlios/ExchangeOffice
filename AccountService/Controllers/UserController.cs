using AccountService.Models;
using AccountService.Models.ViewModels;
using AccountService.Repositories;
using AccountService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly JwtTokenService _jwtTokenService;

        public UserController(UserRepository userRepository, JwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userRepository.GetByNameAsync(model.Name);
            if (existingUser != null)
                return Conflict(new { Message = "User already exists." });

            var user = new User
            {
                Name = model.Name,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            await _userRepository.AddAsync(user);
            var token = _jwtTokenService.GenerateToken(user.Id, user.Name);

            return Ok(new { Message = "Registration successful.", Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userRepository.GetByNameAsync(model.Name);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                return Unauthorized(new { Message = "Invalid credentials." });

            var token = _jwtTokenService.GenerateToken(user.Id, user.Name);
            return Ok(new { Message = "Login successful.", Token = token });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { Message = "Logout successful (handled on client)." });
        }
    }
}