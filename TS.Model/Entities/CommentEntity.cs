using System;
using System.ComponentModel.DataAnnotations.Schema;
using TS.Model.Common;

namespace TS.Model.Domain
{
    public class CommentEntity : BaseEntity
    {
        public string Message { get; set; }

        [ForeignKey("Task")]
        public Guid TaskId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        // Navigation properties
        public TaskEntity Task { get; set; }

        public UserEntity User { get; set; }
    }
}