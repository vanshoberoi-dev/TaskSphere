using System.ComponentModel.DataAnnotations;

namespace TS.Contract.DTOs.Task
{
    public class DeleteTaskRequestDTO
    {
        [Required]
        public required int TaskId { get; set; }

        public bool ForceDelete { get; set; } = false;
    }
}