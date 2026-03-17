using TS.Contract.DTOs.Comment;

namespace TS.ServiceLogic.Interfaces
{
    public interface ICommentService
    {
        Task<AddCommentResponseDTO> AddCommentAsync(AddCommentRequestDTO request);

        Task<ICollection<CommentResponseDTO>> ShowCommentsAsync(int taskId);

        Task<DeleteCommentResponseDTO> DeleteCommentAsync(int commentId);
    }
}