using TS.Contract.DTOs.Task;

public interface ITaskService
{
    Task<CreateTaskResponseDTO> CreateTaskAsync(CreateTaskRequestDTO request);
    //Task<CreateTaskResponseDTO> AssignTaskAsync(AssignTaskRequestDTO request);
    //Task<IEnumerable<CreateTaskResponseDTO>> GetAllTasksAsync();
    //Task<CreateTaskResponseDTO?> GetTaskByIdAsync(int taskId);
    Task<bool> UpdateTaskStatusAsync(UpdateTaskStatusRequestDTO request);
    Task<bool> DeleteTaskAsync(DeleteTaskRequestDTO request);

}