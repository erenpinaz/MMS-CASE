using System.Net;
using Catalog.API.Infrastructure.DTOs;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/products")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IProductService _productService;

    public ProductsController(ILogger<ProductsController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProductsAsync()
    {
        var products = await _productService.GetProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProductAsync(long id)
    {
        var product = await _productService.GetProduct(id);
        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = await _productService.CreateProduct(productDto);
        return Ok(product);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto productDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = await _productService.UpdateProduct(productDto);
        return Ok(product);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProductById(long id)
    {
        await _productService.DeleteProduct(id);
        return Ok();
    }
}
