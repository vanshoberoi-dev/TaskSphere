using ServiceLogic.DTOs.Task;
using TS.Contract.DTOs.Task;

public interface ITaskService
{
    Task<CreateTaskResponseDTO> CreateTaskAsync(CreateTaskRequestDTO request);
    Task<CreateTaskResponseDTO> GetTaskDetailsAsync(int Id);
    //Task<CreateTaskResponseDTO> AssignTaskAsync(AssignTaskRequestDTO request);
    Task<string> ChangeTaskStatusAsync(ChangeTaskStatusRequestDTO request);
    Task<string> DeleteTaskAsync(DeleteTaskRequestDTO request);
    Task<string> UpdateTaskAsync(UpdateTaskRequestDTO request);
}