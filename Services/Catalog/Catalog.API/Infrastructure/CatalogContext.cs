using Catalog.API.Infrastructure.Entities;
using Catalog.API.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Infrastructure;

public class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        builder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.Now.ToUniversalTime();
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAt = DateTime.Now.ToUniversalTime();
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
