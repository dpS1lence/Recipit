namespace Recipit.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Services.Products;
    using Recipit.ViewModels.Product;

    [Route("products/manage")]
    public class ProductController(IProductService productService) : AdministratorController
    {
        private readonly IProductService _productService = productService;

        [HttpPut]
        public async Task<IActionResult> Edit(ProductViewModel model) => Json(await _productService.Edit(model));

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) => Json(await _productService.Delete(id));
    }
}
