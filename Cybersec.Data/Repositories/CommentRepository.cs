using Cybersec.Data.DbContexts;
using Cybersec.Domain.Entities;
using Cybersec.Data.IRepositories;

namespace Cybersec.Data.Repositories;
public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(DataContext dbContext) : base(dbContext)
    {
    }
}
