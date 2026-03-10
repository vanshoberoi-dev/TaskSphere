using TS.Contract.DTOs.Comment;

namespace TS.ServiceLogic.ServiceInterfaces
{
    public interface ICommentService
    {
        Task<string> AddCommentAsync(AddCommentRequestDTO request);
        Task<List<CommentResponseDTO>> ShowCommentsAsync(int taskId);

        Task<string> DeleteCommentAsync(int commentId);
    }
}
