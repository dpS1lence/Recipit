using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recipit.Contracts.Helpers;
using Recipit.Infrastructure.Data;
using Recipit.Infrastructure.Data.Models;

namespace Recipit.Services.Ratings
{
    public class RatingService(RecipitDbContext context, UserManager<Comment> userManager, IHttpContextAccessor httpContextAccessor)
    : IRatingService
    {
        private readonly RecipitDbContext _context = context;
        private readonly UserManager<Comment> _userManager = userManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task Rate(decimal value, int recipeId)
        {
            var user = await GetUser.Data(_userManager, _httpContextAccessor);
            var recipe = await _context.Recipes.FirstOrDefaultAsync(a => a.Id == recipeId);

            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(recipe);

            if (await _context.Ratings
                .FirstOrDefaultAsync(a => a.UserId == user.Id && a.RecipeId == recipeId) is null)
            {
                var rating = new Rating { Value = value, UserId = GetUser.Id(_httpContextAccessor), RecipeId = recipeId };

                await _context.Ratings.AddAsync(rating);
            }
            else
            {
                var userRating = await _context.Ratings
                    .FirstOrDefaultAsync(a => a.UserId == user.Id && a.RecipeId == recipeId);

                ArgumentNullException.ThrowIfNull(userRating);

                userRating.Value = value;
            }

            await _context.SaveChangesAsync();
        }
    }
}
