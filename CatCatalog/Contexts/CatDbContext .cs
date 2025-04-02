using CatCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace CatCatalog.Contexts;

public class CatDbContext : DbContext
{
    public CatDbContext()
        : base()
    {

    }

    public CatDbContext(DbContextOptions<CatDbContext> options)
        : base(options)
    {

    }

    public virtual DbSet<Cat> Cat { get; set; }

    public virtual DbSet<Tag> Tag { get; set; }

    public virtual DbSet<Job> Job { get; set; }

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