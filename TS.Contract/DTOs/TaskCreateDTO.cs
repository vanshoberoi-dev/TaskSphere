using System;

namespace TS.Contract.DTOs
{
    public class TaskCreateDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Guid UserId { get; set; }
    }
}