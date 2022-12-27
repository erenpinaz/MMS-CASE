namespace CaseMms.Shared.EventBus;

public record ProductChangedEvent
{
    public Guid MessageId { get; set; }
    public long ProductId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }
    public string CategoryName { get; set; }
    public bool IsLive { get; set; }

    public ProductChangedEvent() { }

    public ProductChangedEvent(Guid messageId, long productId, string title, string description,
        int stockQuantity, string categoryName, bool isLive)
    {
        MessageId = messageId;
        ProductId = productId;
        Title = title;
        Description = description;
        StockQuantity = stockQuantity;
        CategoryName = categoryName;
        IsLive = isLive;
    }
}