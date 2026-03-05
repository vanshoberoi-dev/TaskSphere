using System;
using TS.Model.Enums;

namespace TS.Model.Domain
{
    public class TaskEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TaskStatus Status { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}