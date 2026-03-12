using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TS.Contract.DTOs.Auth;
using TS.Model.Data;
using TS.Model.Entities.Auth;
using TS.ServiceLogic.Interfaces;

namespace TS.ServiceLogic.Services
{
    public class AuthService : IAuthService
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        // Constructor
        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Function Definitions

        public async Task<string> LoginUserAsync(LoginUserRequestDTO request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !Argon2.Verify(user.PasswordHash, request.Password))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<CreateRoleResponseDTO> CreateRoleAsync(CreateRoleRequestDTO request)
        {
            var role = new RoleEntity
            {
                Role = request.Role
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            return new CreateRoleResponseDTO
            {
                Message = $"Role {role.Role} Successfully created with id {role.Id}"
            };
        }


        public async Task<string> RegisterUserAsync(RegisterUserRequestDTO request)
        {
            
            RoleEntity role = null;
            if (request.RoleId > 0)
            {
                role = await _context.Roles.FindAsync(request.RoleId);
            }

            if (role == null)
            {
                role = await _context.Roles.FirstOrDefaultAsync(r => r.Role == "User");
                if (role == null)
                {
                    role = new RoleEntity { Id = 2 , Role = "User" };
                    await _context.Roles.AddAsync(role);
                    await _context.SaveChangesAsync();
                }
            }

            var user = new UserEntity()
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = Argon2.Hash(request.Password),
                RoleId = role.Id
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return $"User Registered Successfully with user id {user.Id}";
        }

        
    }
}