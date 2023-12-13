using Recipit.Infrastructure.Mapping;

namespace Recipit.Models.Account
{
    public class LoginViewModel
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
