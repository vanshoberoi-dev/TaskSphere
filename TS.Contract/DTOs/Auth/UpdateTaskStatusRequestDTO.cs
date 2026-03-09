namespace TS.Contract.DTOs.Task
{
    public class UpdateTaskStatusRequestDTO
    {
        public int TaskId { get; set; }
        public TS.Contract.Enums.TaskStatus TaskStatus { get; set; }
    }
}