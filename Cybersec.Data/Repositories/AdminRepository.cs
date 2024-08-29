using Cybersec.Data.DbContexts;
using Cybersec.Data.IRepositories;
using Cybersec.Domain.Entities;

namespace Cybersec.Data.Repositories;
public class AdminRepository : Repository<Admin>, IAdminRepository
{
    public AdminRepository(DataContext dbContext) : base(dbContext)
    {
    }
}
