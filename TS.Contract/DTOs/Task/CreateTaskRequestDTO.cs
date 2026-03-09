using System.ComponentModel.DataAnnotations;

public class CreateTaskRequestDTO
{
    [Required]
    [StringLength(100, MinimumLength =3)]
    public string Title { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public TS.Contract.Enums.TaskStatus Status { get; set; } = TS.Contract.Enums.TaskStatus.Pending;

    [Required]
    public required int DueInDays { get; set; }

    public string? Remarks { get; set; } = string.Empty;

}