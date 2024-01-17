using Cybersec.Domain.Entities;
using Cybersec.Data.DbContexts;
using Cybersec.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cybersec.Data.Repositories;
public class UserCodeRepository : IUserCodeRepository
{
    private readonly DataContext _context;

    public UserCodeRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.UserCodes.FirstOrDefaultAsync(x => x.Id == id);

         _context.UserCodes.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<UserCode> InsertAsync(UserCode code)
    {
        var result = await _context.UserCodes.AddAsync(code);
        await _context.SaveChangesAsync();

        return result.Entity;
    }

    public  IQueryable<UserCode> SelectAll()
    => _context.UserCodes.AsQueryable();

    public async Task<UserCode> SelectByIdAsync(int id)
    => await _context.UserCodes.FirstOrDefaultAsync(u => u.Id == id);

}
