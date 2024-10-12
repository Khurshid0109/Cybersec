using Cybersec.Service.DTOs.Comment;

namespace Cybersec.Service.Interfaces.Articles;
public interface ICommentService
{
    Task<ICollection<CommentViewModel>> GetAllCommentsAsync();
    Task<CommentViewModel> GetCommentByIdAsync(long id);
    Task<CommentViewModel> CreateCommentAsync(CommentPostModel model);
    Task<CommentViewModel> UpdateCommentAsync(long id, CommentPutModel model);
    Task<bool> DeleteCommentAsync(long id);
}
