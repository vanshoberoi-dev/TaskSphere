using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TS.ServiceLogic.ServiceInterfaces;
using TS.Contract.DTOs.Comment;
using Microsoft.AspNetCore.Authorization;

namespace TS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [Authorize]
        [HttpPost("add-comment")]
        public async Task<IActionResult> AddComment(AddCommentRequestDTO request)
        {
            try
            {
                return Ok(await _commentService.AddCommentAsync(request));
            }
            catch(Exception ex) {
                return BadRequest($"Error occured : {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("get-comments/{Id}")]
        public async Task<IActionResult> ShowComments(int Id)
        {
            try {
                return Ok(await _commentService.ShowCommentsAsync(Id));
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occured : {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("delete-comment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                var result = await _commentService.DeleteCommentAsync(commentId);
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }
    }
}
