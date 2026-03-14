using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TS.ServiceLogic.Interfaces;
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
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { error = message });
            }
        }

        [HttpGet("show-comments/{TaskId}")]
        public async Task<IActionResult> ShowComments(int TaskId)
        {
            try
            {
                return Ok(await _commentService.ShowCommentsAsync(TaskId));
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { error = message });
            }
        }

       
        [Authorize]
        [HttpPut("delete-comment/{CommentId}")]
        public async Task<IActionResult> DeleteComment(int CommentId)
        {
            try
            {
                var result = await _commentService.DeleteCommentAsync(CommentId);
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { error = message });
            }
        }
    }
}
