using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Http;
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
using static TS.ServiceLogic.Common.Exceptions;

namespace TS.ServiceLogic.Services
{
    public class AuthService : IAuthService
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }


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
            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Role == request.Role);

            if (existingRole != null)
            {
                return new CreateRoleResponseDTO
                {
                    Message = $"Role '{request.Role}' already exists with ID {existingRole.Id}."
                };
            }

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
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new InvalidOperationException("A user with this Email already exists.");
            }

            var user = new UserEntity
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = Argon2.Hash(request.Password),
                RoleId = request.RoleId > 0 ? request.RoleId : 2,
                CreatedOn = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return $"User Registered Successfully with ID {user.Id}";
        }

        public async Task<ICollection<GetUsersResponseDTO>> GetUsersAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .Select(u => new GetUsersResponseDTO
                {
                    Id = u.Id,
                    Username = u.Name,
                    Email = u.Email,
                    Role = u.Role.Role
                })
                .ToListAsync();
        }

        public async Task<string> DeleteUserAsync(DeleteUserRequestDTO request)
        {
          
            TS.ServiceLogic.Common.Utility.ValidateAdminAndGetId(_httpContextAccessor.HttpContext?.User);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
                throw new NotFoundException("User not found");

            if (user.IsDeleted)
                throw new NotFoundException("User is already deleted");

            var activeTasks = await _context.Tasks.AnyAsync(t =>
                t.AssigneeId == user.Id &&
                t.Status != TS.Contract.Enums.TaskStatus.Completed);

            if (activeTasks)
                return "User cannot be deleted because they still have active tasks.";

            user.IsDeleted = true;

            await _context.SaveChangesAsync();

            return $"User '{user.Email}' deleted successfully.";
        }
    }
}