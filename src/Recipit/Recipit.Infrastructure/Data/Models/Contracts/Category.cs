namespace Recipit.Infrastructure.Data.Models.Contracts
{
    public static class Category
    {
        public const string Appetizer = "Предястие";
        public const string Salad = "Салата";
        public const string Soup = "Супа";
        public const string MainCourse = "Основно ястие";
        public const string Dessert = "Десерт";

        public static bool HasCategory(string category) =>new List<string> { Appetizer, Salad, Soup, MainCourse, Dessert }.Contains(category);
    }
}
