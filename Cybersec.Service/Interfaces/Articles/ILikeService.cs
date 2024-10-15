using Cybersec.Service.DTOs.Like;

namespace Cybersec.Service.Interfaces.Articles;
public interface ILikeService
{
    Task<LikeViewModel> CreateLikeAsync(LikePostModel model);
    Task<bool> DeleteLikeAsync(long articleId,long userId);
    Task<bool> IsLikedBefore(long userId, long articleId);
}