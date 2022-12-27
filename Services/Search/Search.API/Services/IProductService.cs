using Search.API.Models;

namespace Search.API.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> SearchProducts(SearchFilter filter);
    Task UpsertProduct(Product product);
    Task DeleteProduct(long productId);
}