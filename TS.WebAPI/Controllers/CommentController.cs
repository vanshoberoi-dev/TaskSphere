using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TS.ServiceLogic.Interfaces;
using TS.Contract.DTOs.Comment;
using Microsoft.AspNetCore.Authorization;
using static TS.ServiceLogic.Common.Exceptions;

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
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message }); // 401
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message }); // 404 (task/user not found)
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message }); // 409 (invalid state)
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpGet("show-comments/{TaskId}")]
        public async Task<IActionResult> ShowComments(int TaskId)
        {
            try
            {
                return Ok(await _commentService.ShowCommentsAsync(TaskId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message }); // task not found
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
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
            catch (UnauthorizedAccessException)
            {
                return Forbid(); // 403 (not allowed)
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message }); // 404
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message }); // 409
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}