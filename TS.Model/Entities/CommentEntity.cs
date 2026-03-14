using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TS.Model.Entities.Auth;

namespace TS.Model.Entities
{
    public class CommentEntity : BaseEntity
    {
        [Required]
        public required int UserId { get; set; }

        [Required]
        public required int TaskId { get; set; }

        [Required, StringLength(2000)]
        public string Comment { get; set; } = string.Empty;


        // --- Navigation properties ---
        
        [ForeignKey("TaskId")]
        public virtual TaskEntity Task { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual UserEntity Commenter { get; set; } = null!;
    }
}