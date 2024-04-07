using Microsoft.AspNetCore.Mvc;
using Recipit.Services.Products;

namespace Recipit.Areas.Home.Controllers
{
    [Area("Home")]
    public class ProductController(IProductService productService) : Controller
    {
        private readonly IProductService _productService = productService;

        [HttpGet]
        public async Task<IActionResult> All() => View(await _productService.All());

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm) => Json(await _productService.Search(searchTerm));
    }
}
