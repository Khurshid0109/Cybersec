using AutoMapper;
using Cybersec.Domain.Entities;
using Cybersec.Data.IRepositories;
using Cybersec.Service.Exceptions;
using Cybersec.Service.DTOs.Comment;
using Microsoft.EntityFrameworkCore;
using Cybersec.Service.Interfaces.Articles;

namespace Cybersec.Service.Services.Articles;
public class CommentService : ICommentService
{
    private readonly IUserRepository _userRepository;
    private readonly IArticleRepository _articleRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;
    public CommentService(IUserRepository userRepository,
                          IArticleRepository articleRepository,
                          ICommentRepository commentRepository,
                          IMapper mapper)
    {
        _userRepository = userRepository;
        _articleRepository = articleRepository;
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<CommentViewModel> CreateCommentAsync(CommentPostModel model)
    {
        var user = await _userRepository.SelectAll()
            .Where(u => u.Id == model.UserId)
            .FirstOrDefaultAsync();

        if(user is null)
            throw new CyberException(404,"User is not found");

        var article = await _articleRepository.SelectAll()
            .Where(a => a.Id == model.ArticleId)
            .FirstOrDefaultAsync();

        if(article is null)
            throw new CyberException(404,"Article is not found");

        var comment = _mapper.Map<Comment>(model);
        comment.CreatedAt = DateTime.UtcNow;

        var result = await _commentRepository.InsertAsync(comment);

        return _mapper.Map<CommentViewModel>(result);
    }

    public async Task<bool> DeleteCommentAsync(long id)
    {
        var comment = await _commentRepository.SelectAll()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();

        if(comment is null)
            throw new CyberException(404,"Comment is not found");

        var result = await _commentRepository.DeleteAsync(id);

        return result;
    }

    public async Task<ICollection<CommentViewModel>> GetAllCommentsAsync()
    {
        var comments = await _commentRepository.SelectAll()
             .AsNoTracking()
             .ToListAsync();

        return _mapper.Map<ICollection<CommentViewModel>>(comments);
    }

    public async Task<CommentViewModel> GetCommentByIdAsync(long id)
    {
        var comment = await _commentRepository.SelectAll()
            .Where(c => c.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if(comment is null)
            throw new CyberException(404,"Comment is not found");

        return _mapper.Map<CommentViewModel>(comment);
    }

    public async Task<CommentViewModel> UpdateCommentAsync(long id, CommentPutModel model)
    {
        var user = await _userRepository.SelectAll()
            .Where(u => u.Id == model.UserId)
            .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "User is not found");

        var article = await _articleRepository.SelectAll()
            .Where(a => a.Id == model.ArticleId)
            .FirstOrDefaultAsync();

        if (article is null)
            throw new CyberException(404, "Article is not found");

        var comment = await _commentRepository.SelectAll()
           .Where(c => c.Id == id)
           .FirstOrDefaultAsync();

        if (comment is null)
            throw new CyberException(404, "Comment is not found");

        var mapped = _mapper.Map(model, comment);
        mapped.UpdatedAt = DateTime.UtcNow;

        var result = await _commentRepository.UpdateAsync(mapped);
        return _mapper.Map<CommentViewModel>(result);
    }
}
