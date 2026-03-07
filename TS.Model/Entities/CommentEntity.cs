using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TS.Model.Entities.Auth;

namespace TS.Model.Entities
{
    public class CommentEntity : BaseEntity
    {
        [Required, StringLength(2000)]
        public string Message { get; set; } = string.Empty; // Fixed CS8618 warning

        [Required]
        public int TaskId { get; set; }

        [Required]
        public int UserId { get; set; }

        // --- Navigation properties ---
        
        [ForeignKey("TaskId")]
        public virtual TaskEntity Task { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual UserEntity User { get; set; } = null!;
    }
}