using TS.Contract.DTOs.Task;

namespace TS.ServiceLogic.Interfaces
{
    public interface ITaskService
    {
        Task<CreateTaskResponseDTO> CreateTaskAsync(CreateTaskRequestDTO request);
        Task<IEnumerable<GetTaskResponseDTO>> GetTasksAsync();
        Task<GetTaskResponseDTO> GetTaskByIDAsync(int Id);
        Task<GeneralResponseDTO> AssignTaskAsync(AssignTaskRequestDTO request);
        Task<GeneralResponseDTO> ChangeTaskStatusAsync(ChangeTaskStatusRequestDTO request);
        Task<GeneralResponseDTO> DeleteTaskAsync(DeleteTaskRequestDTO request);
        Task<GeneralResponseDTO> UpdateTaskAsync(UpdateTaskRequestDTO request);
    }
}
