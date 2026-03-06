using System;

namespace TS.Contract.DTOs
{
    public class CommentResponseDTO
    {

        public string Message { get; set; }

        public Guid TaskId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}