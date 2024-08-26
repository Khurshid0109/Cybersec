using Cybersec.Data.DbContexts;
using Cybersec.Data.IRepositories;
using Cybersec.Domain.Entities;

namespace Cybersec.Data.Repositories;
public class UserCodeRepository : Repository<UserCode>, IUserCodeRepository
{
    public UserCodeRepository(DataContext dbContext) : base(dbContext)
    {
    }
}