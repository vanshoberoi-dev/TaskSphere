using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TS.Contract.Enums;
using TS.Model.Entities;
using TS.Model.Entities.Auth;

public class TaskEntity : BaseEntity
{

    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public TS.Contract.Enums.TaskStatus Status { get; set; }

    [Required]
    public required DateTime DueDate { get; set; }

    [Required]
    public required string CreatedByAdminEmail { get; set; }

    public string? AssignedToUserEmail { get; set; }

    // Navigation property

    [ForeignKey("AssignedToUserId")]
    public virtual UserEntity Assignee{ get; set; }


    [ForeignKey("CreatedByAdminId")]
    public virtual UserEntity Admin { get; set; }
}