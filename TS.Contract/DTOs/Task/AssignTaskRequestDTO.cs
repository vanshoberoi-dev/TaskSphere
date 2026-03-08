using System.ComponentModel.DataAnnotations;

public class AssignTaskRequestDTO
{
    [Required]
    public required int TaskId { get; set; }

    [Required]
    public required int AssigneeEmail { get; set; }
}