namespace Recipit.Infrastructure.Extensions
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Recipit.Contracts.Constants;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Infrastructure.Extensions.Contracts;

    public static class ApplicationBuilderExtensions
    {
        public static void CreateAdministratorUser(this IApplicationBuilder app, IConfiguration configuration)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<RecipitUser>>();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var mapper = serviceScope.ServiceProvider.GetRequiredService<IMapper>();

            if (!roleManager.RoleExistsAsync(RecipitRole.Administrator).Result)
            {
                roleManager.CreateAsync(new IdentityRole(RecipitRole.Administrator)).Wait();
            }

            var adminSettings = configuration.GetSection("UserSettings").Get<UserSettings>();

            var administratorUser = userManager.FindByNameAsync(adminSettings?.UserName 
                ?? throw new ArgumentException("Invalid appsettings!")).Result;

            if (administratorUser == null)
            {
                var adminUser = mapper.Map<RecipitUser>(adminSettings);

                var result = userManager.CreateAsync(adminUser, adminSettings.Password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUser, RecipitRole.Administrator).Wait();
                }
            }
        }
    }
}
