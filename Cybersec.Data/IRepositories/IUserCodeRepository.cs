
using Cybersec.Domain.Entities;

namespace Cybersec.Data.IRepositories;
public interface IUserCodeRepository
{
    IQueryable<UserCode> SelectAll();
    Task<UserCode> InsertAsync(UserCode code);
    Task<bool> DeleteAsync(int id);
    Task<UserCode> SelectByIdAsync(int id);
}
