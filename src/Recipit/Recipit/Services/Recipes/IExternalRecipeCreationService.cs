namespace Recipit.Services.Recipes
{
    public interface IExternalRecipeCreationService
    {
        Task<int> Create(string adress);
    }
}
