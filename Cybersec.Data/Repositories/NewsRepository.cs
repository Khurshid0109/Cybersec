using Cybersec.Domain.Entities;
using Cybersec.Data.DbContexts;
using Cybersec.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cybersec.Data.Repositories;
public class NewsRepository : INewsRepository
{
    private readonly DataContext _context;

    public NewsRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var news = await _context.Information.FirstOrDefaultAsync(n => n.Id == id);

        _context.Information.Remove(news);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<News> InsertAsync(News news)
    {
        var entry = await _context.Information.AddAsync(news);

        await _context.SaveChangesAsync();

        return entry.Entity;
    }

    public IQueryable<News> SelectAll()
    => _context.Information.AsQueryable();

    public async Task<News> SelectByIdAsync(int id)
    => await _context.Information.FirstOrDefaultAsync(n => n.Id == id);

    public async Task<News> UpdateAsync(News news)
    {
        var entry = _context.Information.Update(news);

        await _context.SaveChangesAsync();
        return entry.Entity;
    }
}
