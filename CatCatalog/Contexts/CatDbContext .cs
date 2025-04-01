using CatCatalog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CatCatalog.Contexts;

public class CatDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public CatDbContext(DbContextOptions<CatDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }
    public DbSet<Cat> Cat { get; set; }

    public DbSet<Tag> Tag { get; set; }

    public DbSet<Job> Job { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cat>()
            .HasIndex(i => i.CatId)
            .IsUnique();

        modelBuilder.Entity<Tag>()
            .HasIndex(i => i.Name)
            .IsUnique();

        modelBuilder.Entity<Cat>()
            .HasMany(e => e.Tags)
            .WithMany(e => e.Cats);

        modelBuilder.Entity<Job>()
            .HasIndex(i => i.IsCompleted);
    }
}