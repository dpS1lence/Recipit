namespace Recipit.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Contracts.Enums;
    using Recipit.Services.Products;
    using Recipit.ViewModels.Product;

    public class ProductController(IProductService productService) : AdministratorController
    {
        private readonly IProductService _productService = productService;

        [HttpGet("/allpaginated")]
        public async Task<IActionResult> AllPaginated(string? name, int pageIndex, bool json)
        {
            if(json)
            {
                return Json(await _productService.AllPaginated(pageIndex, name));
            }

            return View(await _productService.AllPaginated(pageIndex, name));
        }
        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] ProductViewModel model) => Json(await _productService.Edit(model));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var name = await _productService.Delete(id);

            return Json(name);
        }
    }
}
