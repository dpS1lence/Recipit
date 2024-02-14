namespace Recipit.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        public string OldPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmNewPassword { get; set; } = default!;
    }
}
