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
    using Recipit.Pagination;
    using Recipit.Pagination.Contracts;
    using Recipit.Services.Images;
    using Recipit.ViewModels.Recipe;
    using System.Collections.Generic;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            Validate.Model(model, _logger);

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
            Validate.Model(recipe, _logger);

            var recipeDbo = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipe.Id);
            Validate.Model(recipeDbo, _logger);

            _context.Entry(recipeDbo!).CurrentValues.SetValues(recipe);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int recipeId)
        {
            var recipeDbo = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);
            Validate.Model(recipeDbo, _logger);

            var comments = await _context.Comments.Where(a => a.RecipeId == recipeId).ToListAsync();

            _context.Recipes.Remove(recipeDbo!);
            _context.Comments.RemoveRange(comments);

            await _context.SaveChangesAsync();
        }

        public async Task<IPage<RecipeDisplayModel>> All(int currentPage, int pageSize)
        {
            var totalRecipesCount = await _context.Recipes.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecipesCount / (double)pageSize);

            var allRecipes = await _context.Recipes
                .Include(a => a.ProductRecipes)
                .ThenInclude(a => a.Product)
                .Include(a => a.User)
                .Include(a => a.Comments)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var recipeViewModels = _mapper.Map<IEnumerable<RecipeDisplayModel>>(allRecipes);

            return new Page<RecipeDisplayModel>(recipeViewModels, currentPage, pageSize, totalPages);
        }

        public async Task<IPage<RecipeDisplayModel>> Filter(RecipeFilterModel model, int currentPage, int pageSize)
        {
            var totalFilteredCount = await _context.Recipes.CountAsync();
            var totalPages = (int)Math.Ceiling(totalFilteredCount / (double)pageSize);

            var filteredRecipes = await _context.Recipes
                .Include(a => a.ProductRecipes)
                .ThenInclude(a => a.Product)
                .Include(a => a.Comments)
                .Include(a => a.User)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (!string.IsNullOrEmpty(model.Name))
            {
                filteredRecipes = filteredRecipes.Where(r => r.Name.Contains(model.Name, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(model.Category))
            {
                filteredRecipes = filteredRecipes.Where(r => r.Category.Contains(model.Category, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(model.Author))
            {
                filteredRecipes = filteredRecipes.Where(r => r.User.UserName.Contains(model.Author, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            if (model.AverageRating.HasValue)
            {
                filteredRecipes = filteredRecipes.Where(r => r.AverageRating >= model.AverageRating.Value).ToList();
            }

            if (model.NutritionalValue.HasValue)
            {
                filteredRecipes = filteredRecipes.Where(r => r.NutritionalValue >= model.NutritionalValue.Value).ToList();
            }

            var recipeViewModels = _mapper.Map<IEnumerable<RecipeDisplayModel>>(filteredRecipes);

            return new Page<RecipeDisplayModel>(recipeViewModels, currentPage, pageSize, totalPages);
        }

        public async Task<RecipeDisplayModel> ById(int id)
        {
            var recipe = await _context.Recipes
                .Where(a => a.Id == id)
                .Include(a => a.User)
                .Include(a => a.Comments)
                .Include(a => a.ProductRecipes)
                .ThenInclude(a => a.Product)
                .FirstOrDefaultAsync();

            return _mapper.Map<RecipeDisplayModel>(recipe);
        }
    }
}