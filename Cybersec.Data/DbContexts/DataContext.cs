using Cybersec.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cybersec.Data.DbContexts;
public class DataContext:DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<News> Information { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserCode> UserCodes { get; set; }
}
