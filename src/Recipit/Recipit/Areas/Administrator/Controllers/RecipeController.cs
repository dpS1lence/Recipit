namespace Recipit.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Services.Recipes;

    [Route("recipes")]
    public class RecipeController(IRecipeService recipeService) : AdministratorController
    {
        private readonly IRecipeService _recipeService = recipeService;

        [HttpPost]
        public async Task Delete([FromBody] int recipeId)
        {
            var name = await _recipeService.Delete(recipeId);

            TempData["message"] = $"Успешно изтрихте {name}!";
        }
    }
}
