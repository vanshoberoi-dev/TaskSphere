using System;
using System.ComponentModel.DataAnnotations.Schema;
using TS.Model.Common;

namespace TS.Model.Domain
{
    public class CommentEntity : BaseEntity
    {
        public string Message { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }

        // Navigation properties

        [ForeignKey("TaskId")]
        public TaskEntity Task { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
    }
}