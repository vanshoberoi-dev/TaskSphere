using System.ComponentModel.DataAnnotations;

public class GetTaskResponseDTO
{
    [Required]
    public required int TaskId { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public TS.Contract.Enums.TaskStatus Status { get; set; } = TS.Contract.Enums.TaskStatus.Pending;

    [Required]
    public required DateTime DueDate { get; set; }

    public string? Remarks { get; set; } = string.Empty;

    [Required]
    public required int CreatedByAdminId { get; set; }

    [Required]
    public required DateTime CreatedOn { get; set; }
}