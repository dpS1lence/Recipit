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
        public IActionResult All() => View();

        [HttpGet]
        public async Task<IActionResult> ViewRecipe(int id) => View(await _recipeService.ById(id));

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

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RecipeViewModel model) => Json(await _recipeService.Create(model));
    }
}
