namespace Recipit.Areas.Follower.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Services.Products;
    using Recipit.ViewModels.Product;

    [Route("products")]
    public class ProductController(IProductService productService) : FollowerController
    {
        private readonly IProductService _productService = productService;

        [HttpGet]
        public async Task<IActionResult> All() => View(await _productService.All());

        [HttpGet]
        public async Task<IActionResult> GetById(int id) => Json(await _productService.GetById(id));

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model) => Json(await _productService.Create(model));
    }
}
