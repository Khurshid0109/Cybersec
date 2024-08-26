using Cybersec.Data.DbContexts;
using Cybersec.Data.IRepositories;
using Cybersec.Domain.Commons;
using Cybersec.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cybersec.Data.Repositories;
public class Repository<TEntity> : IRepository<TEntity> where TEntity : Auditable
{
    protected readonly DataContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(DataContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }


    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        var entry = await _dbSet.AddAsync(entity);

        await _dbContext.SaveChangesAsync();

        return entry.Entity;
    }


    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        entity.Status = Status.Deleted;

        return await _dbContext.SaveChangesAsync() > 0;
    }


    public IQueryable<TEntity> SelectAll()
        => _dbSet;

    public async Task<TEntity> SelectByIdAsync(long id)
        => await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var entry = _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();

        return entry.Entity;
    }

    public async Task<bool> RollbackAsync(long id)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        entity.Status = Status.Active;
        return await _dbContext.SaveChangesAsync() > 0;
    }
}
