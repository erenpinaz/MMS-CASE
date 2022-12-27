using Catalog.API.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations;

class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .UseHiLo("categoryseq")
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(c => c.MinStockQuantity)
            .IsRequired();
    }
}
