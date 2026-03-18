namespace TS.Contract.DTOs.Task
{
    public class UpdateTaskRequestDTO
    {
        public int TaskId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int DueInDays { get; set; }

        public TS.Contract.Enums.TaskStatus Status { get; set; }

        public int AssignedToUserId { get; set; }

        public string Remarks { get; set; }
    }
}