using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TS.Contract.DTOs.Comment;
using TS.Model.Data;
using TS.Model.Entities;
using TS.ServiceLogic.ServiceInterfaces;

namespace TS.ServiceLogic.ServiceImplementations
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommentService(IHttpContextAccessor httpContextAccessor,AppDbContext context) {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<string> AddCommentAsync(AddCommentRequestDTO request)
        {
            var task = await _context.Tasks.AnyAsync(t => t.Id == request.TaskId);
            if (!task)
            {
                throw new KeyNotFoundException($"Task with ID {request.TaskId} not found.");
            }

            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User not authenticated.");

            var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user identity.");
            }

            var newComment = new CommentEntity
            {
                UserId =userId,
                TaskId = request.TaskId,
                Comment = request.Comment
                
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            return $"Commented !  Comment ID : {newComment.Id}";
        }

        public async Task<List<CommentResponseDTO>> ShowCommentsAsync(int taskId)
        {
            var task = await _context.Tasks.AnyAsync(t => t.Id == taskId);
            if (!task)
            {
                throw new KeyNotFoundException($"Task with ID {taskId} not found.");
            }

            var comments = await _context.Comments
                .AsNoTracking()
                .Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.CreatedOn)
                .Select(c => new CommentResponseDTO
                {
                    CommentID = c.Id,
                    CommenterEmail = c.Commenter.Email,
                    TaskTitle = c.Task.Title,
                    Comment = c.Comment,
                    CreatedOn = c.CreatedOn
                })
                .ToListAsync();

            return comments;
        }

        public async Task<string> DeleteCommentAsync(int commentId)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int currentUserId))
            {
                throw new UnauthorizedAccessException("Invalid user identity.");
            }

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                throw new KeyNotFoundException($"Comment with ID {commentId} not found.");
            }

            if (comment.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this comment.");
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return "Comment deleted successfully.";
        }
    }
}
