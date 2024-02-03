namespace Recipit.ViewModels.Recipe
{
    public class RecipeFilterModel
    {
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public string Author { get; set; } = default!;
        public decimal? AverageRating { get; set; }
        public int? NutritionalValue { get; set; }
    }
}
