using System.ComponentModel.DataAnnotations;

namespace TS.Contract.DTOs.Task
{
    public class AssignTaskRequestDTO
    {
        [Required]
        public required int TaskId { get; set; }

        [Required]
        public required string AssigneeEmail { get; set; }

        public bool ForcedAssign { get; set; }
    }
}