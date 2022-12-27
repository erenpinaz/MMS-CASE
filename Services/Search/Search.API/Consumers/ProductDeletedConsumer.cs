using System.Diagnostics;
using CaseMms.Shared.EventBus;
using MassTransit;
using MassTransit.Metadata;
using Search.API.Services;

namespace Search.API.Consumers;

public class ProductDeletedConsumer : IConsumer<ProductDeletedEvent>
{
    private readonly ILogger<ProductDeletedConsumer> _logger;
    private readonly IProductService _productService;

    public ProductDeletedConsumer(ILogger<ProductDeletedConsumer> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
    {
        var timer = Stopwatch.StartNew();

        try
        {
            await _productService.DeleteProduct(context.Message.ProductId);

            _logger.LogInformation($"Receive client: [Message: {context.Message.MessageId}, Type: {typeof(ProductDeletedEvent)}]");
            await context.NotifyConsumed(timer.Elapsed, TypeMetadataCache<ProductDeletedEvent>.ShortName);
        }
        catch (Exception ex)
        {
            await context.NotifyFaulted(timer.Elapsed, TypeMetadataCache<ProductDeletedEvent>.ShortName, ex);
        }
    }
}

public class ProductDeletedConsumerDefinition : ConsumerDefinition<ProductDeletedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ProductDeletedConsumer> consumerConfigurator)
    {
        consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
    }
}