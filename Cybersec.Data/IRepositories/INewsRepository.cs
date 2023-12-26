using Cybersec.Domain.Entities;

namespace Cybersec.Data.IRepositories;
public interface INewsRepository
{
    IQueryable<News> SelectAll();
    Task<News> InsertAsync(News news);
    Task<bool> DeleteAsync(int id);
    Task<News> UpdateAsync(News news);
    Task<News> SelectByIdAsync(int id);
}
