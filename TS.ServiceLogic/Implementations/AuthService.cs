using Microsoft.EntityFrameworkCore;
using TS.Contract;
using TS.Model.Data;
using TS.Model.Entities;
using TS.ServiceLogic.Interfaces;

namespace TS.ServiceLogic.Implementations
{
    public class AuthService : IAuthService
    {

        private readonly AppDbContext _context;

        public async Task<string> RegisterAsync(RegisterRequestDTO dto)
        {
            return "User Registered Successfully";
        }

        public async Task<RoleResponseDTO> CreateRoleAsync(CreateRoleRequestDTO dto)
        {
            var role = new RoleEntity
            {
                Role = dto.Role
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            return new RoleResponseDTO
            {
                Id = role.Id,
                RoleName = role.Role
            };
        }
    }
}