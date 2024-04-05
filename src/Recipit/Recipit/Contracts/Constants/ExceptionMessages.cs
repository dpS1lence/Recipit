namespace Recipit.Contracts.Constants
{
    public class ExceptionMessages
    {
        public class Recipe
        {
            public const string NameIsNullOrEmpty = "Моля въведете име на рецептата!";
            public const string DescriptionIsNullOrEmpty = "Моля въведете описание на рецептата!";
            public const string ProductListIsNull = "Моля изберете продукти!";
            public const string ProductListIsEmpty = "Моля добавете продукти!";
            public const string PhotoIsInvalid = "Моля изберете подходяща снимка!";
            public const string CaloriesIsNullOrEmpty = "Моля въведете калории на рецептата!";
            public const string CategoryIsNullOrEmpty = "Моля изберете категория за рецептата!";
            public const string ProductDoesNotExist = "Моля изберете валиден продукт!";
            public const string AlreadyExists = "Рецептата вече съществува!";
            public const string ProductMustHaveValue = "Продуктът трябва да има описание!";
        }
    }
}
