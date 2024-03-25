﻿namespace Recipit.Areas.Administrator.Controllers
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
        public async Task<IActionResult> Edit([FromForm] ProductViewModel model)
        {
            TempData["message"] = $"Успешно редактирахте {model.Name}!";

            return Json(await _productService.Edit(model));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var name = await _productService.Delete(id);

            TempData["message"] = $"Успешно изтрихте {name}!";

            return Json(name);
        }
    }
}
