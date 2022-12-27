using AutoMapper;
using CaseMms.Shared.EventBus;
using Catalog.API.Infrastructure;
using Catalog.API.Infrastructure.DTOs;
using Catalog.API.Infrastructure.Entities;
using Catalog.API.Infrastructure.Exceptions;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publisher;
    private readonly CatalogContext _context;

    public ProductService(IMapper mapper, IPublishEndpoint publisher, CatalogContext context)
    {
        _mapper = mapper;
        _publisher = publisher;
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto> GetProduct(long productId)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .SingleOrDefaultAsync(p => p.Id == productId);
        if (product == null)
            throw new NotFoundException("Specified product doesn't exists");

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateProduct(CreateProductDto productDto)
    {
        var category = await _context.Categories.SingleOrDefaultAsync(c => c.Id == productDto.CategoryId);
        if (category == null)
            throw new NotFoundException("Specified category doesn't exists");

        var product = _mapper.Map<Product>(productDto);

        var entry = await _context.Products.AddAsync(product);
        var result = await _context.SaveChangesAsync() > 0;
        if (result)
        {
            var message = _mapper.Map<ProductChangedEvent>(entry.Entity);
            await _publisher.Publish(message);
        }

        return _mapper.Map<ProductDto>(entry.Entity);
    }

    public async Task<ProductDto> UpdateProduct(UpdateProductDto productDto)
    {
        var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == productDto.Id);
        if (product == null)
            throw new NotFoundException("Specified product doesn't exist");

        var category = await _context.Categories.SingleOrDefaultAsync(c => c.Id == productDto.CategoryId);
        if (category == null)
            throw new NotFoundException("Specified category doesn't exists");

        var updatedProduct = _mapper.Map(productDto, product);
        var entry = _context.Products.Update(updatedProduct);
        var result = await _context.SaveChangesAsync() > 0;
        if (result)
        {
            var message = _mapper.Map<ProductChangedEvent>(entry.Entity);
            await _publisher.Publish(message);
        }

        return _mapper.Map<ProductDto>(entry.Entity);
    }

    public async Task DeleteProduct(long productId)
    {
        var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == productId);
        if (product == null)
            throw new NotFoundException("Specified product doesn't exist");

        var entry = _context.Products.Remove(product);
        var result = await _context.SaveChangesAsync() > 0;
        if (result)
        {
            var message = _mapper.Map<ProductDeletedEvent>(entry.Entity);
            await _publisher.Publish(message);
        }
    }
}