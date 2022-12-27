namespace Catalog.API.Infrastructure.Entities;

public class Category : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public int MinStockQuantity { get; set; }
}