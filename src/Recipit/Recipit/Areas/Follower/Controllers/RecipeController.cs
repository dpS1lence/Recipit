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

        [HttpGet]
        public async Task<IActionResult> All() => View(await _recipeService.All());

        [HttpGet]
        public async Task<IActionResult> Filter([FromForm] RecipeFilterModel model) => Json(await _recipeService.Filter(model));

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RecipeViewModel model) => Json(await _recipeService.Create(model));
    }
}
