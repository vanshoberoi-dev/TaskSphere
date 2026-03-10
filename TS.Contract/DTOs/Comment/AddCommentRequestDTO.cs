using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
