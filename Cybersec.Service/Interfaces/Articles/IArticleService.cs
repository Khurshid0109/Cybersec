using Cybersec.Service.ViewModels.Article;

namespace Cybersec.Service.Interfaces.Articles;
public interface IArticleService
{
    Task<ArticleViewModel> CreateArticleAsync(ArticlePostModel model);
    Task<ArticleViewModel> GetArticleByIdAsync(long id);
    Task<ICollection<ArticleViewModel>> GetAllArticlesAsync();
    Task<ArticleViewModel> UpdateArticleAsync(long id,ArticlePutModel model);
    Task<bool> DeleteArticleAsync(long id);
}
