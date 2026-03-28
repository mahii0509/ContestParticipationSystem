using System.Security.Cryptography;
using System.Text;
using ContestParticipationSystem.Data;
using ContestParticipationSystem.DTOs;
using ContestParticipationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ContestParticipationSystem.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthService(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

      
        public async Task<User> RegisterAsync(RegisterDTO registerDto)
        {
        
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new Exception("Email already exists");

       
            var hashedPassword = HashPassword(registerDto.Password);

            var user = new User
            {
                Username = registerDto.Username,  
                Email = registerDto.Email,
                Password = hashedPassword,
                Role = registerDto.Role           
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

       
        public async Task<string> LoginAsync(LoginDTO loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
                throw new Exception("Invalid credentials");

          
            return _jwtService.GenerateToken(user);
        }

     
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

      
        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }
}