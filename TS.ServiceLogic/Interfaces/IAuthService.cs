using TS.Contract.DTOs.Auth;

namespace TS.ServiceLogic.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(RegisterUserRequestDTO request);
        Task<CreateRoleResponseDTO> CreateRoleAsync(CreateRoleRequestDTO request);
        Task<LoginUserResponseDTO> LoginUserAsync(LoginUserRequestDTO request);
        Task<ICollection<GetUsersResponseDTO>> GetUsersAsync();
        Task<DeleteUserResponseDTO> DeleteUserAsync(DeleteUserRequestDTO request);
    }
}