using System.Diagnostics;
using CaseMms.Shared.EventBus;
using MassTransit;
using MassTransit.Metadata;
using Search.API.Models;
using Search.API.Services;

namespace Search.API.Consumers;

public class ProductChangedConsumer : IConsumer<ProductChangedEvent>
{
    private readonly ILogger<ProductChangedConsumer> _logger;
    private readonly IProductService _productService;

    public ProductChangedConsumer(ILogger<ProductChangedConsumer> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task Consume(ConsumeContext<ProductChangedEvent> context)
    {
        var timer = Stopwatch.StartNew();

        try
        {
            await _productService.UpsertProduct(new Product
            {
                Id = context.Message.ProductId,
                Title = context.Message.Title,
                Description = context.Message.Description,
                CategoryName = context.Message.CategoryName,
                StockQuantity = context.Message.StockQuantity,
                IsLive = context.Message.IsLive
            });

            _logger.LogInformation($"Receive client: [Message: {context.Message.MessageId}, Type: {typeof(ProductChangedEvent)}]");
            await context.NotifyConsumed(timer.Elapsed, TypeMetadataCache<ProductChangedEvent>.ShortName);
        }
        catch (Exception ex)
        {
            await context.NotifyFaulted(timer.Elapsed, TypeMetadataCache<ProductChangedEvent>.ShortName, ex);
        }
    }
}

public class ProductChangedConsumerDefinition : ConsumerDefinition<ProductChangedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ProductChangedConsumer> consumerConfigurator)
    {
        consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
    }
}