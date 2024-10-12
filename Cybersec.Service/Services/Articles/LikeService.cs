using AutoMapper;
using Cybersec.Data.IRepositories;
using Cybersec.Domain.Entities;
using Cybersec.Service.DTOs.Like;
using Cybersec.Service.Exceptions;
using Cybersec.Service.Interfaces.Articles;
using Microsoft.EntityFrameworkCore;

namespace Cybersec.Service.Services.Articles;
public class LikeService : ILikeService
{
    private readonly ILikeRepository _likeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;

    public LikeService(ILikeRepository likeRepository,
                       IUserRepository userRepository,
                       IMapper mapper,
                       IArticleRepository articleRepository)
    {
        _likeRepository = likeRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _articleRepository = articleRepository;
    }

    public async Task<LikeViewModel> CreateLikeAsync(LikePostModel model)
    {
       var user = await _userRepository.SelectAll()
            .Where(u => u.Id == model.UserId)
            .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "User is not found.");

        var article = await _articleRepository.SelectAll()
            .Where(a => a.Id == model.ArticleId)
            .FirstOrDefaultAsync();

        if (article is null)
            throw new CyberException(404, "Article is not found.");

        var mapped = _mapper.Map<Like>(model);
        mapped.CreatedAt = DateTime.UtcNow;

        var result = await _likeRepository.InsertAsync(mapped);

        return _mapper.Map<LikeViewModel>(result);
    }

    public async Task<bool> DeleteLikeAsync(long id)
    {
        var like = await _likeRepository.SelectAll()
             .Where(l => l.Id == id)
             .FirstOrDefaultAsync();

        if (like is null)
            throw new CyberException(404, "Like is not found.");

        var result = await _likeRepository.DeleteAsync(id);
        return result;
    }

    public async Task<bool> IsLikedBefore(long userId,long articleId)
    {
       var like = await _likeRepository.SelectAll()
            .Where(l => l.UserId == userId && l.ArticleId == articleId)
            .FirstOrDefaultAsync();

        return like is not null;
    }
}
