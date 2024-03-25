using Recipit.Pagination.Contracts;
using Recipit.ViewModels;
using Recipit.ViewModels.Recipe;

namespace Recipit.Services.Recipes
{
    public interface IRecipeService
    {
        Task<string> Delete(int recipeId);
        Task Edit(RecipeViewModel recipe);
        Task<EditRecipeOutputModel> EditById(int id);
        Task<IPage<RecipeOutputModel>> All(int currentPage, int pageSize);
        Task<IPage<RecipeOutputModel>> Filter(RecipeFilterModel model, int currentPage, int pageSize);
        Task<string> Create(RecipeViewModel recipe);
        Task<RecipeDisplayModel> ById(int id, bool isUserAuthenticated);
        Task<HomePageViewModel> GetHomePage();
    }
}
