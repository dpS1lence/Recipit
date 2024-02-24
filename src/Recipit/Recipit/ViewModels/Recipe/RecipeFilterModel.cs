using Recipit.Contracts.Enums;

namespace Recipit.ViewModels.Recipe
{
    public class RecipeFilterModel
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Author { get; set; }
        public SortDirection? AverageRating { get; set; }
        public SortDirection? NutritionalValue { get; set; }
    }
}
