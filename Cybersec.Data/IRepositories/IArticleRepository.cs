using Cybersec.Domain.Entities;

namespace Cybersec.Data.IRepositories;
public interface IArticleRepository
{
    IQueryable<Article> SelectAll();
    Task<Article> InsertAsync(Article article);
    Task<bool> DeleteAsync(long id);
    Task<Article> UpdateAsync(Article article);
    Task<Article> SelectByIdAsync(long id);
}
