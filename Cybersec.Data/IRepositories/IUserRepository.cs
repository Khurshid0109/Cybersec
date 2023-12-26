using Cybersec.Domain.Entities;

namespace Cybersec.Data.IRepositories;
public interface IUserRepository
{
    IQueryable<User> SelectAll();
    Task<User> InsertAsync(User user);
    Task<bool> DeleteAsync(int id);
    Task<User> UpdateAsync(User user);
    Task<User> SelectByIdAsync(int id);
}
