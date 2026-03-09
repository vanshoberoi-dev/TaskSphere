namespace TS.Contract.DTOs.Task
{
    public class ChangeTaskStatusRequestDTO
    {
        public int TaskId { get; set; }
        public TS.Contract.Enums.TaskStatus TaskStatus { get; set; }
    }
}