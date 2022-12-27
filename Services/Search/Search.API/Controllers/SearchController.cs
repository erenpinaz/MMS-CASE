using System.Net;
using Microsoft.AspNetCore.Mvc;
using Search.API.Models;
using Search.API.Services;

namespace Search.API.Controllers;

[ApiController]
[Route("api/v1/search")]
public class SearchController : ControllerBase
{
    private readonly ILogger<SearchController> _logger;
    private readonly IProductService _productService;

    public SearchController(ILogger<SearchController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get([FromQuery] SearchFilter filter)
    {
        var products = await _productService.SearchProducts(filter);
        return Ok(products);
    }
}
