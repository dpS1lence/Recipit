﻿namespace Recipit.Areas.Follower.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Services.Ratings;
    using Recipit.Services.Recipes;
    using Recipit.ViewModels.Recipe;

    public class RecipeController(IRecipeService recipeService, IRatingService ratingService, IExternalRecipeCreationService externalRecipeService) : FollowerController
    {
        private readonly IRecipeService _recipeService = recipeService;
        private readonly IRatingService _ratingService = ratingService;
        private readonly IExternalRecipeCreationService _externalRecipeService = externalRecipeService;

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RecipeViewModel model) => Json(await _recipeService.Create(model));

        [HttpPost]
        public async Task<IActionResult> CreateExternaly(string url)
        {
            var resultId = await _externalRecipeService.Create(url);

            return RedirectToAction("ViewRecipe", "Recipe", new { id = resultId, area = "Home" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _recipeService.Delete(id);

            return Redirect("/profile");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) => View(await _recipeService.EditById(id));

        [HttpGet]
        public async Task Rate(int value, int recipeId) => await _ratingService.Rate(value, recipeId);

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] RecipeViewModel model)
        {
            await _recipeService.Edit(model);

            return RedirectToAction("ViewRecipe", "Recipe", new { id = model.Id, area = "Home" });
        }
    }
}
