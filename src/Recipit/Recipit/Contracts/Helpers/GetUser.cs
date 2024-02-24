using Microsoft.AspNetCore.Identity;
using Recipit.Infrastructure.Data.Models;
using System.Security.Claims;

namespace Recipit.Contracts.Helpers
{
    public static class GetUser
    {
        public static async Task<RecipitUser> Data(UserManager<RecipitUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            var uId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            return await userManager.FindByIdAsync(uId)
                ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public static string Id(IHttpContextAccessor httpContextAccessor) => httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }
}
