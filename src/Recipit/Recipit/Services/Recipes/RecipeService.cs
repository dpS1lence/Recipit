namespace Recipit.Services.Recipes
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Recipit.Contracts;
    using Recipit.Contracts.Exceptions;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Services.Images;
    using Recipit.ViewModels.Recipe;
    using System.Net.Http.Headers;
    using System.Security.Claims;

    public class RecipeService
        (RecipitDbContext context, UserManager<RecipitUser> userManager, HttpClient httpClient, ILogger<RecipeService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : IRecipeService
    {
        private readonly RecipitDbContext _context = context;
        private readonly UserManager<RecipitUser> _userManager = userManager;
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<string> Create(RecipeViewModel model)
        {
            Validate.Entity(model, _logger);

            var userId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId ?? throw new ArgumentException("User not found!"));

            var recipe = _mapper.Map<Recipe>(model);
            recipe.User = user ?? throw new ArgumentException("No products selected!");
            recipe.UserId = userId;
            recipe.Photo = await UploadImage.ToImgur(model.Photo, _httpClient);

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            foreach (var item in model?.ProductNames ?? throw new ArgumentException("No products selected!"))
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Name == item);

                if (product == null) 
                    throw new ProductNotFoundException(nameof(product));

                recipe.NutritionalValue += product.Calories;

                var productRecipe = new ProductRecipe
                {
                    Product = product,
                    ProductId = product.Id,
                    Recipe = recipe,
                    RecipeId = recipe.Id
                };

                _context.ProductsRecipies.Add(productRecipe);
            }

            await _context.SaveChangesAsync();

            return recipe.Name;
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
