using System.ComponentModel.DataAnnotations;

namespace Recipit.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        public string OldPassword { get; set; } = default!;

        [MinLength(8)]
        public string NewPassword { get; set; } = default!;
        public string ConfirmNewPassword { get; set; } = default!;
    }
}
