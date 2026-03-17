using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Contract.DTOs.Comment
{
    public class AddCommentResponseDTO
    {
            public  required string Message { get; set; } 
            public required int CommentId { get; set; }  
    }
}
