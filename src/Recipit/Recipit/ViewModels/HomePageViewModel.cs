using Recipit.ViewModels.Recipe;

namespace Recipit.ViewModels
{
    public class HomePageViewModel
    {
        public int FollowersCount { get; set; }

        public int RecipesCount { get; set; }

        public RecipeDisplayModel RecipeOfTheDay { get; set; } = default!;

        public IEnumerable<RecipeDisplayModel> LatestRecipes { get; set; } = default!;
        public IEnumerable<RecipeDisplayModel> TopRatedRecipes { get; set; } = default!;
    }
}
