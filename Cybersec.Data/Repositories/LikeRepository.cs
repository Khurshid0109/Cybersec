using Cybersec.Data.DbContexts;
using Cybersec.Domain.Entities;
using Cybersec.Data.IRepositories;

namespace Cybersec.Data.Repositories;
public class LikeRepository : Repository<Like>, ILikeRepository
{
    public LikeRepository(DataContext dbContext) : base(dbContext)
    {
    }
}
