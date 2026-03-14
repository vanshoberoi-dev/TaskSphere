using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TS.Contract.DTOs.Comment;
using TS.Model.Data;
using TS.Model.Entities;
using TS.ServiceLogic.Common;
using TS.ServiceLogic.Interfaces;

namespace TS.ServiceLogic.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<string> AddCommentAsync(AddCommentRequestDTO request)
        {
            var taskExists = await _context.Tasks.AnyAsync(t => t.Id == request.TaskId);
            if (!taskExists)
            {
                return $"Task with ID {request.TaskId} not found.";
            }

            int userId = Utility.ValidateUserAndGetId(_httpContextAccessor.HttpContext?.User);

            var newComment = new CommentEntity
            {
                UserId = userId,
                TaskId = request.TaskId,
                Comment = request.Comment
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            return $"Commented ! Comment ID : {newComment.Id}";
        }

        public async Task<List<CommentResponseDTO>> ShowCommentsAsync(int taskId)
        {
            var taskExists = await _context.Tasks.AnyAsync(t => t.Id == taskId);
            if (!taskExists)
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
            int currentUserId = Utility.ValidateUserAndGetId(_httpContextAccessor.HttpContext?.User);

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            
            if (comment == null)
            {
                return $"Comment with ID {commentId} not found.";
            }

            if (comment.UserId != currentUserId)
            {
                return "You do not have permission to delete this comment.";
            }

            comment.IsDeleted = true;
            await _context.SaveChangesAsync();

            return "Comment deleted successfully.";
        }
    }
}