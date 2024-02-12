namespace Recipit.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Services.Products;
    using Recipit.ViewModels.Product;

    public class ProductController(IProductService productService) : AdministratorController
    {
        private readonly IProductService _productService = productService;

        [HttpGet]
        public async Task<IActionResult> All() => View(await _productService.All());

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] ProductViewModel model) => Json(await _productService.Edit(model));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int id) => Json(await _productService.Delete(id));
    }
}
