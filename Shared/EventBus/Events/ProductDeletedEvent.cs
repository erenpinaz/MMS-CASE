namespace CaseMms.Shared.EventBus;

public record ProductDeletedEvent
{
    public Guid MessageId { get; set; }
    public long ProductId { get; set; }

    public ProductDeletedEvent() { }

    public ProductDeletedEvent(Guid messageId, long productId)
    {
        MessageId = messageId;
        ProductId = productId;
    }
}