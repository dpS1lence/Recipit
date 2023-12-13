using Microsoft.AspNetCore.Identity;
using Recipit.Infrastructure.Data.Models;
using Recipit.Infrastructure.Extensions.Contracts;

namespace Recipit.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void CreateAdministratorUser(this IApplicationBuilder app, IConfiguration configuration)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<RecipitUser>>();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roleExists = roleManager.RoleExistsAsync("Administrator").Result;
            if (!roleExists)
            {
                roleManager.CreateAsync(new IdentityRole("Administrator")).Wait();
            }

            var adminSettings = configuration.GetSection("AdministratorUser").Get<UserSettings>();

            var administratorUser = userManager.FindByNameAsync(adminSettings?.UserName ?? throw new ArgumentException("Invalid appsetting!")).Result;
            if (administratorUser == null)
            {
                var adminUser = new RecipitUser
                {
                    UserName = adminSettings.UserName,
                    FirstName = adminSettings.FirstName,
                    LastName = adminSettings.LastName,
                    Email = adminSettings.Email,
                    EmailConfirmed = adminSettings.EmailConfirmed,
                    Photo = adminSettings.Photo
                };

                var result = userManager.CreateAsync(adminUser, adminSettings.Password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUser, "Administrator").Wait();
                }
            }
        }
    }
}
