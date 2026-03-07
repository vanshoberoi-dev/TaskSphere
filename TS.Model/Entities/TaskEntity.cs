using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TS.Model.Entities.Auth;

namespace TS.Model.Entities
{
    public class TaskEntity : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public TS.Contract.Enums.TaskStatus Status { get; set; } = TS.Contract.Enums.TaskStatus.Pending;  

        [Required]
        public int UserId { get; set; }

        // Navigation Property
        [ForeignKey("UserId")]
        public virtual UserEntity User { get; set; }
    }
}