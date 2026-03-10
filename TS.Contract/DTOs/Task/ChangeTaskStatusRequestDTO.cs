using System.ComponentModel.DataAnnotations;

namespace TS.Contract.DTOs.Task
{
    public class ChangeTaskStatusRequestDTO
    {
        [Required]
        public required int TaskId { get; set; }
        [Required]
        public required TS.Contract.Enums.TaskStatus TaskStatus { get; set; }
    }
}