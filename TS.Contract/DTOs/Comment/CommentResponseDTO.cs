using System.ComponentModel.DataAnnotations;

namespace TS.Contract.DTOs.Comment
{
    public class CommentResponseDTO
    {
        [Required]
        public required int CommentID { get; set; }

        [Required]
        public required string CommenterEmail { get; set; }

        [Required]
        public required string TaskTitle { get; set; }

        [Required]
        public required string Comment { get; set; }

        [Required]
        public required DateTime CreatedOn { get; set; }
    }
}