using System.ComponentModel.DataAnnotations;

namespace TS.Contract.DTOs.Task
{
    public class TaskCreateDTO
    {
        [Required]
        public required string Title { get; set; }

        public string? Description { get; set; }

        public int UserId { get; set; }
    }
}