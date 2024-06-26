﻿using Microsoft.AspNetCore.Mvc;
using Recipit.Services.Recipes;
using Recipit.ViewModels.Recipe;

namespace Recipit.Areas.Home.Controllers
{
    [Area("Home")]
    public class RecipeController(IRecipeService recipeService) : Controller
    {
        private readonly IRecipeService _recipeService = recipeService;

        [HttpGet]
        public IActionResult All() => View();

        [HttpGet]
        public async Task<IActionResult> ViewRecipe(int id) 
            => View(await _recipeService.ById(id, User?.Identity?.IsAuthenticated ?? false));

        [HttpGet]
        public async Task<IActionResult> Recipes(int currentPage, int pageSize)
        {
            var pageOfRecipes = await _recipeService.All(currentPage, pageSize);

            return Json(new { recipes = pageOfRecipes, totalPages = pageOfRecipes.TotalCount });
        }

        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] RecipeFilterModel model, int currentPage, int pageSize)
        {
            var pageOfRecipes = await _recipeService.Filter(model, currentPage, pageSize);

            return Json(new { recipes = pageOfRecipes, totalPages = pageOfRecipes.TotalCount });
        }
    }
}
