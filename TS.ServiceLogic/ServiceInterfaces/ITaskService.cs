using ServiceLogic.DTOs.Task;
using TS.Contract.DTOs.Task;

public interface ITaskService
{
    Task<CreateTaskResponseDTO> CreateTaskAsync(CreateTaskRequestDTO request);
    Task<IEnumerable<GetTaskResponseDTO>> GetTasksAsync();
    Task<GetTaskResponseDTO> GetTaskByIDAsync(int Id);
    Task<string> AssignTaskAsync(AssignTaskRequestDTO request);
    Task<string> ChangeTaskStatusAsync(ChangeTaskStatusRequestDTO request);
    Task<string> DeleteTaskAsync(DeleteTaskRequestDTO request);
    Task<string> UpdateTaskAsync(UpdateTaskRequestDTO request);
}