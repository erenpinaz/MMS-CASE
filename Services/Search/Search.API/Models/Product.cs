namespace Search.API.Models;

public class Product
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string CategoryName { get; set; }
    public int StockQuantity { get; set; }
    public bool IsLive { get; set; }
}