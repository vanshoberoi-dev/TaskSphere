using System.ComponentModel.DataAnnotations;

namespace TS.Contract.DTOs.Comment
{
    public class AddCommentRequestDTO
    {
        [Required]
        public required int TaskId { get; set; }

        [Required]
        public required string Comment { get; set; }
    }
}
