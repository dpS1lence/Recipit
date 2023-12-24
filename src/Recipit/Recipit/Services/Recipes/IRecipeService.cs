using Recipit.ViewModels.Recipe;

namespace Recipit.Services.Recipes
{
    public interface IRecipeService
    {
        Task Delete(int recipeId);
        Task Edit(RecipeViewModel recipe);
        Task Create(RecipeViewModel recipe);
    }
}
