using Cybersec.Data.DbContexts;
using Cybersec.Domain.Entities;
using Cybersec.Data.IRepositories;

namespace Cybersec.Data.Repositories;
public class PassResetTokenRepository : Repository<PasswordResetToken>, IPassResetTokenRepository
{
    public PassResetTokenRepository(DataContext dbContext) : base(dbContext)
    {
    }
}
