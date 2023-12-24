namespace Recipit.Services.Recipes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Recipit.Contracts;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.ViewModels.Recipe;

    public class RecipeService
        (RecipitDbContext context, ILogger<RecipeService> logger, IMapper mapper)
        : IRecipeService
    {
        private readonly RecipitDbContext _context = context;
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task Create(RecipeViewModel model)
        {
            Validate.Entity(model, _logger);

            var recipe = _mapper.Map<Recipe>(model);

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(RecipeViewModel recipe)
        {
            Validate.Entity(recipe, _logger);

            var recipeDbo = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipe.Id);
            Validate.Entity(recipeDbo, _logger);

            _context.Entry(recipeDbo!).CurrentValues.SetValues(recipe);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int recipeId)
        {
            var recipeDbo = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);
            Validate.Entity(recipeDbo, _logger);

            var comments = await _context.Comments.Where(a => a.RecipeId == recipeId).ToListAsync();

            _context.Recipes.Remove(recipeDbo!);
            _context.Comments.RemoveRange(comments);

            await _context.SaveChangesAsync();
        }
    }
}
