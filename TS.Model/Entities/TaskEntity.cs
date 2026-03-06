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
        public TS.Model.Enums.TaskStatus Status { get; set; }
        public int UserId { get; set; }
        // Navigation Property

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
    }
}