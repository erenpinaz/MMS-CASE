namespace Catalog.API.Infrastructure.DTOs;

public class BaseProductDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }
}

public class CreateProductDto : BaseProductDto
{
    public int CategoryId { get; set; }
}

public class UpdateProductDto : BaseProductDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
}

public class ProductDto : BaseProductDto
{
    public long Id { get; set; }
    public string Category { get; set; }
}