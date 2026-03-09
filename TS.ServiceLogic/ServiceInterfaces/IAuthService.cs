using TS.Contract.DTOs.Auth;
using TS.Contract.DTOs.Task;

namespace TS.ServiceLogic.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(RegisterUserRequestDTO request);
        Task<CreateRoleResponseDTO> CreateRoleAsync(CreateRoleRequestDTO request);
        Task<string> LoginUserAsync(LoginUserRequestDTO request);
    }
}