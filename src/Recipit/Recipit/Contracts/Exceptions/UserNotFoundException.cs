namespace Recipit.Contracts.Exceptions
{
    public class UserNotFoundException(string message) : Exception(message)
    { }
}
