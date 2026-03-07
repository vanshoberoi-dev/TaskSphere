using System;
using System.ComponentModel.DataAnnotations;

namespace TS.Contract.DTOs.Comment
{
    public class CommentResponseDTO
    {
        [Required]
        public required string Message { get; set; }

        [Required]
        public required int TaskId { get; set; }

        [Required]
        public required int UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}