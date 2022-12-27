namespace Catalog.API.Infrastructure.Entities;

public class Product : BaseEntity
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }
}