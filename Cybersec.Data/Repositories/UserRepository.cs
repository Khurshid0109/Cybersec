using Cybersec.Data.DbContexts;
using Cybersec.Data.IRepositories;
using Cybersec.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cybersec.Data.Repositories;
public class UserRepository:IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        _context.Users.Remove(user);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<User> InsertAsync(User user)
    {
        var entry = await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public IQueryable<User> SelectAll()
    => _context.Users.AsQueryable();

    public async Task<User> SelectByIdAsync(int id)
    => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User> UpdateAsync(User user)
    {
        var res = _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return res.Entity;
    }
}
