using Cybersec.Data.DbContexts;
using Cybersec.Domain.Entities;
using Cybersec.Data.IRepositories;

namespace Cybersec.Data.Repositories;
public class ArticleRepository : Repository<Article>, IArticleRepository
{
    public ArticleRepository(DataContext dbContext) : base(dbContext)
    {
    }
}
