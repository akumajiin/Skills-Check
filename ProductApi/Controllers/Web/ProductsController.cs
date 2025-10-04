using Microsoft.AspNetCore.Mvc;

namespace ProductApi.Controllers.Web
{
    [Route("web/[controller]")]
    public class ProductsController : Controller
    {
    private readonly IProductRepository _repo;

    public ProductsController(IProductRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("index")]
    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var items = await _repo.GetAllAsync(page, pageSize);
        ViewData["Page"] = page;
        ViewData["PageSize"] = pageSize;
        return View(items);
    }
    }
}
