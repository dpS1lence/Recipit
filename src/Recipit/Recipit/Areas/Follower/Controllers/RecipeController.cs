namespace Recipit.Areas.Follower.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Services.Recipes;
    using Recipit.ViewModels.Recipe;

    public class RecipeController(IRecipeService recipeService) : FollowerController
    {
        private readonly IRecipeService _recipeService = recipeService;

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RecipeViewModel model) => Json(await _recipeService.Create(model));

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _recipeService.Delete(id);

            return Redirect("/profile");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) => View(await _recipeService.EditById(id));

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] RecipeViewModel model)
        {
            await _recipeService.Edit(model);

            return RedirectToAction("ViewRecipe", "Recipe", new { id = model.Id, area = "Home" });
        }
    }
}
