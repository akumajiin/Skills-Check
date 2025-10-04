using Microsoft.AspNetCore.Mvc;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
    private readonly IProductRepository _repo;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductRepository repo, ILogger<ProductsController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        try
        {
            var products = await _repo.GetAllAsync(page, pageSize, cancellationToken);
            return Ok(products);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Request was cancelled");
            return StatusCode(499); // Client Closed Request (non-standard)
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching products");
            return StatusCode(500, "An error occurred fetching products.");
        }
    }
    }
}

