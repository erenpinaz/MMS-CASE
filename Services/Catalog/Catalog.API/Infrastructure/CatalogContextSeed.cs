using Catalog.API.Infrastructure.Entities;

namespace Catalog.API.Infrastructure;

public class CatalogContextSeed
{
    public static async Task SeedAsync(CatalogContext context, ILogger<CatalogContextSeed> logger)
    {
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(GetPreconfiguredCategories());
            await context.SaveChangesAsync();
            logger.LogInformation("Seed database associated with context {DbContextName}", typeof(CatalogContext).Name);
        }
    }

    private static IEnumerable<Category> GetPreconfiguredCategories()
    {
        return new List<Category>
            {
                new Category() {
                    Name = "Electronic",
                    Slug = "electronic",
                    MinStockQuantity = 3
                },
                new Category() {
                    Name = "Home & Kitchen",
                    Slug = "home-kitchen",
                    MinStockQuantity = 1
                },
                new Category() {
                    Name = "Sports & Outdoor",
                    Slug = "sports-outdoor",
                    MinStockQuantity = 5
                }
            };
    }
}
