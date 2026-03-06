using System.ComponentModel.DataAnnotations.Schema;

namespace TS.Model.Entities
{
    public class TaskEntity : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public TS.Contract.Enums.TaskStatus Status { get; set; }

        public int UserId { get; set; }

        // Navigation Property
        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
    }
}