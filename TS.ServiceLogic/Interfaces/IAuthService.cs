using TS.Contract;

namespace TS.ServiceLogic.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequestDTO dto);
        Task<RoleResponseDTO> CreateRoleAsync(CreateRoleRequestDTO dto);
    }
}