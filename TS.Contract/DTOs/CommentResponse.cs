using System;

namespace TS.Contract.DTOs
{
    public class CommentResponse
    {
        public Guid Id { get; set; }

        public string Message { get; set; }

        public Guid TaskId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}