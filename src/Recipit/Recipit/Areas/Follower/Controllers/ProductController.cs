namespace Recipit.Areas.Follower.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Services.Products;
    using Recipit.ViewModels.Product;

    public class ProductController(IProductService productService) : FollowerController
    {
        private readonly IProductService _productService = productService;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductViewModel model)
        {
            TempData["message"] = $"Успешно създадохте {model.Name}!";

            return Json(await _productService.Create(model));
        }
    }
}
