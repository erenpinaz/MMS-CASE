using Catalog.API.Infrastructure.DTOs;

namespace Catalog.API.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProducts();
    Task<ProductDto> GetProduct(long productId);
    Task<ProductDto> CreateProduct(CreateProductDto productDto);
    Task<ProductDto> UpdateProduct(UpdateProductDto productDto);
    Task DeleteProduct(long productId);
}