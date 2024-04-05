namespace Recipit.Services.Ratings
{
    public interface IRatingService
    {
        Task Rate(decimal value, int recipeId);
    }
}
