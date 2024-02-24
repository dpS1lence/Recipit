using Recipit.Infrastructure.Mapping;

namespace Recipit.ViewModels.Account
{
    public class EditProfileInputModel
    {
        public string Email { get; set; } = default!;
        public IFormFile Photo { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }
}
