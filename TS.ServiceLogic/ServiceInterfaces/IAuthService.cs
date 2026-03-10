using TS.Contract.DTOs.Auth;

namespace TS.ServiceLogic.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(RegisterUserRequestDTO request);
        Task<CreateRoleResponseDTO> CreateRoleAsync(CreateRoleRequestDTO request);
        Task<string> LoginUserAsync(LoginUserRequestDTO request);
    }
}