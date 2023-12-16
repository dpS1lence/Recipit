namespace Recipit.Infrastructure.Extensions.Contracts
{
    public class UserSettings
    {
        public string UserName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public bool EmailConfirmed { get; set; } = default!;
        public string Photo { get; set; } = default!;
    }
}
