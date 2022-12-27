using MassTransit;
using Moq;

namespace Catalog.UnitTest;

public class ProductServiceTests : IDisposable
{
    private readonly CatalogContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publisher;

    public ProductServiceTests()
    {
        var options = new DbContextOptionsBuilder<CatalogContext>()
            .UseInMemoryDatabase(databaseName: $"CatalogDb")
            .Options;

        _dbContext = new CatalogContext(options);
        _mapper = new MapperConfiguration(cfg => { cfg.AddProfile(new ProductProfile()); }).CreateMapper();
        _publisher = Mock.Of<IPublishEndpoint>();

        _dbContext.Database.EnsureDeleted();

        _dbContext.AddRange(GetFakeCategories());
        _dbContext.AddRange(GetFakeProducts());
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task Get_products()
    {
        //Arrange
        var expectedType = typeof(List<ProductDto>);
        var expectedProductCount = 3;

        var productService = new ProductService(_mapper, _publisher, _dbContext);

        //Act
        var result = await productService.GetProducts();

        //Assert
        Assert.IsType(expectedType, result);
        Assert.Equal(expectedProductCount, result.Count());
    }

    [Fact]
    public async Task Get_not_existing_product()
    {
        //Arrange
        var expectedException = typeof(NotFoundException);

        var productService = new ProductService(_mapper, _publisher, _dbContext);

        //Act
        var action = async () => await productService.GetProduct(5);

        //Assert
        await Assert.ThrowsAsync(expectedException, action);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    private List<Category> GetFakeCategories()
    {
        return new List<Category>()
        {
            new()
            {
                Id = 1,
                Name = "Electronic",
                Slug = "electronic",
                MinStockQuantity = 5
            },
        };
    }

    private List<Product> GetFakeProducts()
    {
        return new List<Product>()
        {
            new()
            {
                Id = 1,
                Title = "Apple iPhone 12 64GB Black",
                Description = "Test product #1 description",
                CategoryId = 1,
                StockQuantity = 5
            },
            new()
            {
                Id = 2,
                Title = "Apple iPhone 12 64GB Silver",
                Description = "Test product #2 description",
                CategoryId = 1,
                StockQuantity = 3
            },
            new()
            {
                Id = 3,
                Title = "Apple iPhone 12 64GB Blue",
                Description = "Test product #3 description",
                CategoryId = 1,
                StockQuantity = 15
            },
        };
    }

}