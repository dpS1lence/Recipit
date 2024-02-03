namespace Recipit.Services.Recipes
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Recipit.Contracts;
    using Recipit.Contracts.Exceptions;
    using Recipit.Contracts.Helpers;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Services.Images;
    using Recipit.ViewModels.Recipe;
    using System.Collections.Generic;
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

            if (model.ProductNames == null)
                throw new ArgumentException("No products selected!");
            else if (model?.ProductNames?.Split(',').ToList() == null)
                throw new ArgumentException("No products selected!");

            var user = await GetUserData.ById(_userManager, _httpContextAccessor);

            var recipe = _mapper.Map<Recipe>(model);
            recipe.User = user;
            recipe.UserId = user.Id;
            recipe.Photo = await UploadImage.ToImgur(model.Photo, _httpClient);

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            foreach (var item in model.ProductNames.Split(',').ToList())
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

        public async Task<IEnumerable<RecipeDisplayModel>> All()
        {
            var allRecipes = await _context.Recipes
                .Include(a => a.ProductRecipes)
                .ThenInclude(a => a.Product)
                .Include(a => a.User)
                .ToListAsync();

            var recipeViewModels = _mapper.Map<IEnumerable<RecipeDisplayModel>>(allRecipes);

            return recipeViewModels;
        }

        public async Task<IEnumerable<RecipeDisplayModel>> Filter(RecipeFilterModel model)
        {
            var query = _context.Recipes
                    .Include(a => a.ProductRecipes)
                    .ThenInclude(a => a.Product)
                    .Include(a => a.User)
                    .AsQueryable();

            if (!string.IsNullOrEmpty(model.Name))
            {
                query = query.Where(r => r.Name.Contains(model.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(model.Category))
            {
                query = query.Where(r => r.Category.Equals(model.Category, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(model.Author))
            {
                query = query.Where(r => r.User.UserName.Contains(model.Author, StringComparison.OrdinalIgnoreCase));
            }

            if (model.AverageRating.HasValue)
            {
                query = query.Where(r => r.AverageRating >= model.AverageRating.Value);
            }

            if (model.NutritionalValue.HasValue)
            {
                query = query.Where(r => r.NutritionalValue >= model.NutritionalValue.Value);
            }

            var filteredRecipes = await query.ToListAsync();
            var recipeViewModels = _mapper.Map<IEnumerable<RecipeDisplayModel>>(filteredRecipes);

            return recipeViewModels;
        }
    }
}