namespace Recipit.Contracts.Exceptions
{
    public class ProductNotFoundException(string productName) : ArgumentNullException(nameof(productName), $"Product with name '{productName}' not found.")
    {
    }
}
