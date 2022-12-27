using MassTransit;
using Microsoft.OpenApi.Models;
using Nest;
using Search.API.Consumers;
using Search.API.Extensions;
using Search.API.Models;
using Search.API.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;
var env = builder.Environment;

// Add services to the container.
{
    services.AddControllers();

    // Add Swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Search.API", Version = "v1" });
        c.DescribeAllParametersInCamelCase();
    });

    // Add RabbitMQ
    services.AddMassTransit(x =>
    {
        x.AddConsumer<ProductChangedConsumer>(typeof(ProductChangedConsumerDefinition));
        x.AddConsumer<ProductDeletedConsumer>(typeof(ProductDeletedConsumerDefinition));

        x.SetKebabCaseEndpointNameFormatter();

        x.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host(config.GetConnectionString("RabbitMq"));
            cfg.ReceiveEndpoint("product-queue", e =>
            {
                e.ConfigureConsumer<ProductChangedConsumer>(ctx);
                e.ConfigureConsumer<ProductDeletedConsumer>(ctx);
            });
        });
    });

    // Add Elasticsearch Client
    services.AddElasticsearch(config);

    // Register business services
    services.AddScoped<IProductService, ProductService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
{
    if (app.Environment.IsDevelopment())
    {
    }

    // Configure Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Search.API v1"));

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
}

app.Run();
