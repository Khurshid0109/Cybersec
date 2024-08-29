using Cybersec.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cybersec.Data.DbContexts;
public class DataContext:DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Article> Articles { get; set; }
    public DbSet<CodeBlock> CodeBlocks { get; set; }
    public DbSet<ContentBlock> ContentBlocks { get; set; }
    public DbSet<ImageBlock> ImageBlocks { get; set; }
    public DbSet<TextBlock> TextBlocks { get; set; }
    public DbSet<VideoBlock> VideoBlocks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<UserCode> UserCodes { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContentBlock>()
            .HasDiscriminator<string>("ContentType")
            .HasValue<TextBlock>("Text")
            .HasValue<ImageBlock>("Image")
            .HasValue<VideoBlock>("Video")
            .HasValue<CodeBlock>("Code");

        base.OnModelCreating(modelBuilder);
    }
}
