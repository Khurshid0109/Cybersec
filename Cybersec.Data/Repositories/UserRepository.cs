using Cybersec.Data.DbContexts;
using Cybersec.Data.IRepositories;
using Cybersec.Domain.Entities;

namespace Cybersec.Data.Repositories;
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(DataContext dbContext) : base(dbContext)
    {
    }
}