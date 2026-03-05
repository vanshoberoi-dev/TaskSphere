using System;
using System.ComponentModel.DataAnnotations.Schema;
using TS.Model.Common;
using TS.Model.Enums;

namespace TS.Model.Domain
{
    public class TaskEntity : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public TaskStatus Status { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        // Navigation Property
        public UserEntity User { get; set; }
    }
}