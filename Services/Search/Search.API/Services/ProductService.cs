using Nest;
using Search.API.Models;

namespace Search.API.Services;

public class ProductService : IProductService
{
    private readonly ElasticClient _elasticClient;

    public ProductService(ElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<IEnumerable<Product>> SearchProducts(SearchFilter filter)
    {
        var response = await _elasticClient.SearchAsync<Product>(s => s
            .From(0)
            .Size(10)
            .Query(q => q
                .Bool(b => b
                    .Must(m => m
                        .MultiMatch(mm => mm
                            .Query(filter.Keyword)
                            .Type(TextQueryType.BoolPrefix)
                            .Operator(Operator.And)
                            .Fields(fs => fs
                                .Field(f => f.Title)
                                .Field(f => f.Description)
                                .Field(f => f.CategoryName))
                        )
                    )
                    .Filter(f => f
                        .Term(te => te
                            .Field(f => f.IsLive)
                            .Value(true)
                        ) & f
                        .Range(ra => ra
                            .Field(f => f.StockQuantity)
                            .GreaterThanOrEquals(filter.MinQuantity)
                            .LessThanOrEquals(filter.MaxQuantity)
                        )
                    )
                )
            )
        );
        if (!response.IsValid)
            throw new Exception("Problem searching for documents");

        return response.Documents?.ToList();
    }

    public async Task UpsertProduct(Product product)
    {
        var response = await _elasticClient.IndexDocumentAsync<Product>(product);
        if (!response.IsValid)
            throw new Exception("Problem inserting document to Elasticsearch.");
    }

    public async Task DeleteProduct(long productId)
    {
        var response = await _elasticClient.DeleteAsync<Product>(productId);
        if (!response.IsValid)
            throw new Exception("Problem deleting document from Elasticsearch.");
    }
}