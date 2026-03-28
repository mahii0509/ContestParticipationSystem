using Microsoft.AspNetCore.Mvc;
using ContestParticipationSystem.Data;
using ContestParticipationSystem.Models;
using ContestParticipationSystem.DTOs;
using ContestParticipationSystem.Services;
using System.Linq;

namespace ContestParticipationSystem.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDTO dto)
        {
            if (_context.Users.Any(x => x.Email == dto.Email))
                return BadRequest("Email already exists");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role ?? "USER"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == dto.Email);

            if (user == null)
                return BadRequest("Invalid email");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return BadRequest("Invalid password");

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                token,
                role = user.Role
            });
        }
    }
}