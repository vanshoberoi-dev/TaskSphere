using System.ComponentModel.DataAnnotations;

namespace TS.Contract.DTOs.Task
{
    public class CreateTaskResponseDTO
    {
        [Required]
        public required string message { get; set; }
    }
}