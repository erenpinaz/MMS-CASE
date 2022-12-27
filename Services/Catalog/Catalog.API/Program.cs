using System.Reflection;
using Catalog.API.Extensions;
using Catalog.API.Infrastructure;
using Catalog.API.Infrastructure.Middlewares;
using Catalog.API.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;
var env = builder.Environment;

// Add services to the container.
{
    services.AddControllers();

    // Add Swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c => { 
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.API", Version = "v1" }); 
        c.DescribeAllParametersInCamelCase();
    });

    // Add Fluent Validation
    services.AddFluentValidationAutoValidation();
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    // Add PostgreSQL
    services.AddDbContext<CatalogContext>(options =>
        options.UseNpgsql(config.GetConnectionString("CatalogConnectionString")));

    // Add AutoMapper
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Add RabbitMQ
    services.AddMassTransit(x =>
    {
        x.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host(config.GetConnectionString("RabbitMq"));
            cfg.UseMessageRetry(retry => { retry.Interval(3, TimeSpan.FromSeconds(5)); });
        });
    });

    // Register business services
    services.AddScoped<IProductService, ProductService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
{
    if (app.Environment.IsDevelopment())
    {
    }

    // Apply postgresql database migrations.
    app.MigrateDatabase<CatalogContext>((context, services) =>
    {
        var logger = services.GetService<ILogger<CatalogContextSeed>>();
        CatalogContextSeed.SeedAsync(context, logger).Wait();
    });

    // Configure error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    // Configure Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
}

app.Run();
