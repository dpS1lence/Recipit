namespace Recipit.Services.Account
{
    using Recipit.ViewModels.Account;

    public interface IAccountService
    {
        Task<UserViewModel> GetByName(string name);
        Task<UserViewModel> GetCurrentUser();
    }
}
