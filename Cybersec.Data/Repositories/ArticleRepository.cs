using Cybersec.Data.DbContexts;
using Cybersec.Domain.Entities;
using Cybersec.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cybersec.Data.Repositories;
public class ArticleRepository : IArticleRepository
{
    private readonly DataContext _context;

    public ArticleRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(n => n.Id == id);

        _context.Articles.Remove(article);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Article> InsertAsync(Article article)
    {
        var entry = await _context.Articles.AddAsync(article);

        await _context.SaveChangesAsync();

        return entry.Entity;
    }

    public IQueryable<Article> SelectAll()
    => _context.Articles.AsQueryable();

    public async Task<Article> SelectByIdAsync(long id)
    {
        var model = await _context.Articles.Where(m => m.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

       return model;
    }

    public async Task<Article> UpdateAsync(Article article)
    {
        var entry = _context.Articles.Update(article);

        await _context.SaveChangesAsync();
        return entry.Entity;
    }
}
