using AutoMapper;
using Cybersec.Domain.Entities;
using Cybersec.Data.IRepositories;
using Cybersec.Service.DTOs.Like;
using Cybersec.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using Cybersec.Service.Interfaces.Articles;
using Cybersec.Domain.Enums;

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

        var like = await _likeRepository.SelectAll()
             .Where(l => l.UserId == model.UserId && l.ArticleId == model.ArticleId && l.Status == Status.Deleted)
             .FirstOrDefaultAsync();
        var result = new Like();

        if (like is not null)
        {
            like.Status = Status.Active;
            result = await _likeRepository.UpdateAsync(like);
        }
        else
        {
            var mapped = _mapper.Map<Like>(model);
            mapped.CreatedAt = DateTime.UtcNow;

            result = await _likeRepository.InsertAsync(mapped);
        }
        return _mapper.Map<LikeViewModel>(result);
    }

    public async Task<bool> DeleteLikeAsync(long articleId,long userId)
    {
        var user = await _userRepository.SelectAll()
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "User is not found.");

        var article = await _articleRepository.SelectAll()
            .Where(a => a.Id == articleId)
            .FirstOrDefaultAsync();

        if (article is null)
            throw new CyberException(404, "Article is not found.");

        var like = await _likeRepository.SelectAll()
             .Where(l => l.UserId == userId && l.ArticleId == articleId && l.Status == Status.Active)
             .FirstOrDefaultAsync();

        if (like is not null)
             await _likeRepository.DeleteAsync(like.Id);
        
        return true;
    }

    public async Task<bool> IsLikedBefore(long userId,long articleId)
    {
       var like = await _likeRepository.SelectAll()
            .Where(l => l.UserId == userId && l.ArticleId == articleId)
            .FirstOrDefaultAsync();

        return like is not null;
    }
}
