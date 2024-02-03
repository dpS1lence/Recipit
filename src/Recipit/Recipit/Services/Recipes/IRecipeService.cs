using Recipit.ViewModels.Recipe;

namespace Recipit.Services.Recipes
{
    public interface IRecipeService
    {
        Task Delete(int recipeId);
        Task Edit(RecipeViewModel recipe);
        Task<IEnumerable<RecipeDisplayModel>> All();
        Task<IEnumerable<RecipeDisplayModel>> Filter(RecipeFilterModel model);
        Task<string> Create(RecipeViewModel recipe);
    }
}
