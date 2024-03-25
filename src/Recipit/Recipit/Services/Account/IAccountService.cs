namespace Recipit.Services.Account
{
    using Recipit.ViewModels.Account;

    public interface IAccountService
    {
        Task ChangePassword(ChangePasswordViewModel model);
        Task DeleteProfile();
        Task<string> DeleteUserById(string uId);
        Task EditProfile(EditProfileInputModel model);
        Task<UserViewModel> GetByName(string name);
        Task<UserViewModel> GetCurrentUser();
    }
}
