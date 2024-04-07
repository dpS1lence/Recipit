using Recipit.Infrastructure.Data.Models.Contracts;

namespace Recipit.Contracts.Constants
{
    public class GotvachBgCategories
    {
        private static readonly string[] appetizers = ["Предястия", "Колбаси", "Карантия", "Кайма", "Снакс", "Солени Печива", "Закуски и Печива", "Палачинки"];
        private static readonly string[] salads = ["Салати", "Трушии и зимнина"];
        private static readonly string[] soups = ["Супи", "Студени Супи", "Крем Супи", "Чорби и супи"];
        private static readonly string[] mainCourses = ["Зеленчукови ястия", "Свинско месо", "Ястия с Яйца", "Ястия с Месо", "Дивеч", "Други", "Тарнитури", "Баница", "Пица", "Паста", "Морски дарове", "Риба"];
        private static readonly string[] desserts = ["Десерти", "Сладолед", "Кремове", "Сладка", "Други Десерти", "Сладки и Бисквити", "Сладкиши", ""];

        public static string GetName(string name)
        {
            if(appetizers.Contains(name))
            {
                return Category.Appetizer;
            }
            else if (salads.Contains(name))
            {
                return Category.Salad;
            }
            else if (soups.Contains(name))
            {
                return Category.Soup;
            }
            else if (mainCourses.Contains(name))
            {
                return Category.MainCourse;
            }
            else if (desserts.Contains(name))
            {
                return Category.Dessert;
            }
            else
            {
                return Category.Appetizer;
            }
        }
    }
}
