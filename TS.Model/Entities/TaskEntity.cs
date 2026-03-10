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
    public TS.Contract.Enums.TaskStatus Status { get; set; } = TS.Contract.Enums.TaskStatus.Pending;

    [Required]
    public required DateTime DueDate { get; set; }

    public string Remarks { get; set; } = string.Empty;

    [Required]
    public required int CreatedByAdminId { get; set; }

    public int? AssigneeId { get; set; }

    // Navigation property

    [ForeignKey("CreatedByAdminId")]
    public virtual UserEntity Admin { get; set; }

    [ForeignKey("AssigneeId")]
    public virtual UserEntity? Assignee { get; set; }

    public virtual ICollection<CommentEntity> Comments { get; set; } = new HashSet<CommentEntity>();
}