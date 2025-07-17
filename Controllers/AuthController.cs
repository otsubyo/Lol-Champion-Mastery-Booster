using Lol_Champion_Mastery_Booster.Data;
using Lol_Champion_Mastery_Booster.DTOs;
using Lol_Champion_Mastery_Booster.Models;
using Lol_Champion_Mastery_Booster.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lol_Champion_Mastery_Booster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext db, JwtService jwtService)
        {
            _db = db;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest("Email already used.");
            }

            var user = new User
            {
                Email = dto.Email,
                // utilisation directe de BCrypt.Net.BCrypt
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Ok("User registered.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}
