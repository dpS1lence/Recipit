namespace Recipit.Services.Recipes
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Recipit.Contracts;
    using Recipit.Contracts.Constants;
    using Recipit.Contracts.Enums;
    using Recipit.Contracts.Helpers;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Infrastructure.Data.Models.Contracts;
    using Recipit.Pagination;
    using Recipit.Pagination.Contracts;
    using Recipit.Services.Images;
    using Recipit.ViewModels;
    using Recipit.ViewModels.Recipe;
    using System.Collections.Generic;
    using RecipeDb = Infrastructure.Data.Models.Recipe;

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

            var dict = new Dictionary<string, string>();

            ArgumentException.ThrowIfNullOrEmpty(model.Name, ExceptionMessages.Recipe.NameIsNullOrEmpty);
            ArgumentException.ThrowIfNullOrEmpty(model.Description, ExceptionMessages.Recipe.DescriptionIsNullOrEmpty);

            if (!string.IsNullOrEmpty(model.Products))
                dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.Products);
            ArgumentNullException.ThrowIfNull(dict, ExceptionMessages.Recipe.ProductListIsNull);
            if (dict.Count == 0)
                throw new ArgumentException(ExceptionMessages.Recipe.ProductListIsEmpty);
            else if (model.Photo == null || model.Photo == default)
                throw new ArgumentException(ExceptionMessages.Recipe.PhotoIsInvalid);
            else if (model.Calories < 0)
                throw new ArgumentException(ExceptionMessages.Recipe.CaloriesIsNullOrEmpty);
            else if (string.IsNullOrEmpty(model.Category) || !Category.HasCategory(model.Category))
                throw new ArgumentException(ExceptionMessages.Recipe.CategoryIsNullOrEmpty);
            else if (await _context.Recipes.FirstOrDefaultAsync(a => a.Name == model.Name) != null)
                throw new ArgumentException(ExceptionMessages.Recipe.AlreadyExists);

            var user = await GetUser.Data(_userManager, _httpContextAccessor);

            var recipe = _mapper.Map<RecipeDb>(model);
            recipe.User = user;
            recipe.UserId = user.Id;
            recipe.Photo = await UploadImage.ToImgur(model.Photo, _httpClient);

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            foreach (var item in dict!)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Name == item.Key);

                ArgumentNullException.ThrowIfNull(product, ExceptionMessages.Recipe.ProductDoesNotExist);
                ArgumentException.ThrowIfNullOrEmpty(item.Value, ExceptionMessages.Recipe.ProductMustHaveValue);

                recipe.NutritionalValue += product.Calories;

                var productRecipe = new ProductRecipe
                {
                    Product = product,
                    ProductId = product.Id,
                    QuantityDetails = item.Value,
                    Recipe = recipe,
                    RecipeId = recipe.Id
                };

                _context.ProductsRecipies.Add(productRecipe);
            }

            await _context.SaveChangesAsync();

            return recipe.Name;
        }

        public async Task Edit(RecipeViewModel recipeViewModel)
        {
            var existingRecipe = await _context.Recipes
                .Include(a => a.ProductRecipes)
                .ThenInclude(a => a.Product)
                .FirstOrDefaultAsync(a => a.Id == recipeViewModel.Id);

            ArgumentNullException.ThrowIfNull(existingRecipe);

            if (existingRecipe!.UserId != GetUser.Id(_httpContextAccessor))
            {
                throw new ArgumentException(nameof(existingRecipe.UserId));
            }

            existingRecipe.Name = recipeViewModel.Name;
            existingRecipe.Description = recipeViewModel.Description;

            if (recipeViewModel.Photo is not null)
            {
                existingRecipe.Photo = await UploadImage.ToImgur(recipeViewModel.Photo, _httpClient);
            }

            existingRecipe.Calories = recipeViewModel.Calories;
            existingRecipe.Category = recipeViewModel.Category;

            var newProductsDict = string.IsNullOrEmpty(recipeViewModel.Products) ? []
            : JsonConvert.DeserializeObject<Dictionary<string, string>>(recipeViewModel.Products);

            ArgumentNullException.ThrowIfNull(newProductsDict);

            existingRecipe.ProductRecipes = existingRecipe.ProductRecipes
                .Where(pr => newProductsDict.ContainsKey(pr.Product.Name)).ToList();

            foreach (var item in newProductsDict)
            {
                var productRecipe = existingRecipe.ProductRecipes
                    .FirstOrDefault(pr => pr.Product.Name == item.Key);

                if (productRecipe is null)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == item.Key);
                    ArgumentNullException.ThrowIfNull(product, nameof(product));

                    existingRecipe.ProductRecipes.Add(new ProductRecipe
                    {
                        Product = product,
                        ProductId = product.Id,
                        QuantityDetails = item.Value,
                        Recipe = existingRecipe,
                        RecipeId = existingRecipe.Id
                    });
                }
                else
                {
                    productRecipe.QuantityDetails = item.Value;
                }
            }

            _context.Recipes.Update(existingRecipe);
            await _context.SaveChangesAsync();
        }


        public async Task<string> Delete(int recipeId)
        {
            var recipeDbo = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);
            Validate.Model(recipeDbo, _logger);

            if (recipeDbo!.UserId != GetUser.Id(_httpContextAccessor))
            {
                if (!await _userManager.IsInRoleAsync(await GetUser.Data(_userManager, _httpContextAccessor), RecipitRole.Administrator))
                {
                    throw new ArgumentException(nameof(recipeDbo.UserId));
                }
            }

            var recipeName = recipeDbo.Name;

            var comments = await _context.Comments.Where(a => a.RecipeId == recipeId).ToListAsync();
            var products = await _context.ProductsRecipies.Where(a => a.RecipeId == recipeId).ToListAsync();
            var ratings = await _context.Ratings.Where(a => a.RecipeId == recipeId).ToListAsync();

            _context.Recipes.Remove(recipeDbo!);
            _context.Comments.RemoveRange(comments);
            _context.Ratings.RemoveRange(ratings);
            _context.ProductsRecipies.RemoveRange(products);

            await _context.SaveChangesAsync();

            return recipeName;
        }

        public async Task<IPage<RecipeOutputModel>> All(int currentPage, int pageSize)
        {
            var totalRecipesCount = await _context.Recipes.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecipesCount / (double)pageSize);

            var allRecipes = await _context.Recipes
                .Include(a => a.Ratings)
                .Include(a => a.ProductRecipes)
                .ThenInclude(a => a.Product)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var recipeViewModels = _mapper.Map<IEnumerable<RecipeOutputModel>>(allRecipes);

            return new Page<RecipeOutputModel>(recipeViewModels, currentPage, pageSize, totalPages);
        }

        public async Task<IPage<RecipeOutputModel>> Filter(RecipeFilterModel model, int currentPage, int pageSize)
        {
            var recipes = await _context.Recipes
                .Include(a => a.User)
                .Include(a => a.Ratings)
                .Include(a => a.ProductRecipes)
                    .ThenInclude(a => a.Product)
                .ToListAsync();

            if (!string.IsNullOrEmpty(model.Name))
            {
                recipes = recipes.Where(r => r.Name.Contains(model.Name, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(model.Category))
            {
                recipes = recipes.Where(r => r.Category.Contains(model.Category, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(model.Author))
            {
                recipes = recipes
                    .Where(r => r.User?.UserName?
                        .Contains(model.Author, StringComparison.CurrentCultureIgnoreCase)
                        ?? throw new ArgumentNullException(nameof(r)))
                    .ToList();
            }

            if (model.AverageRating == SortDirection.Ascending)
            {
                recipes = [.. recipes.OrderBy(r => r.AverageRating)];
            }
            else if (model.AverageRating == SortDirection.Descending)
                recipes = [.. recipes.OrderByDescending(r => r.AverageRating)];

            if (model.NutritionalValue == SortDirection.Ascending)
            {
                recipes = [.. recipes.OrderBy(r => r.Calories)];
            }
            else if (model.NutritionalValue == SortDirection.Descending)
                recipes = [.. recipes.OrderByDescending(r => r.Calories)];

            var totalFilteredCount = recipes.Count;
            var totalPages = (int)Math.Ceiling(totalFilteredCount / (double)pageSize);

            var filtered = recipes
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var recipeViewModels = _mapper.Map<IEnumerable<RecipeOutputModel>>(filtered);

            return new Page<RecipeOutputModel>(recipeViewModels, currentPage, pageSize, totalPages);
        }

        public async Task<RecipeDisplayModel> ById(int id, bool isUserAuthenticated)
        {
            var recipe = await _context.Recipes
                .Where(a => a.Id == id)
                .Include(a => a.User)
                .Include(a => a.Comments)
                .ThenInclude(a => a.User)
                .Include(a => a.Ratings)
                .ThenInclude(a => a.User)
                .Include(a => a.ProductRecipes)
                .ThenInclude(a => a.Product)
                .FirstOrDefaultAsync();

            var map = _mapper.Map<RecipeDisplayModel>(recipe);

            if (isUserAuthenticated)
            {
                var userRating = await _context.Ratings.FirstOrDefaultAsync(a => a.UserId == GetUser.Id(_httpContextAccessor) && a.RecipeId == id);

                if (userRating != null)
                {
                    map.UserRating = (int)userRating.Value;
                }
            }

            return map;
        }

        public async Task<HomePageViewModel> GetHomePage()
        {
            var model = new HomePageViewModel();

            var recipesOnDate = await _context.Recipes
                .Where(a => a.PublishDate.Date == DateTime.UtcNow.Date)
                .Include(a => a.Ratings)
                .Include(a => a.User)
                .Include(a => a.ProductRecipes)
                    .ThenInclude(a => a.Product)
                .ToListAsync();

            var dailyRecipe = recipesOnDate
                .OrderByDescending(a => a.AverageRating)
                .FirstOrDefault();

            if (dailyRecipe == null)
            {
                int count = await _context.Recipes.CountAsync();
                var randomIndex = new Random().Next(0, count);
                dailyRecipe = await _context.Recipes
                    .Skip(randomIndex)
                    .Take(1)
                    .Include(a => a.Ratings)
                    .Include(a => a.User)
                    .Include(a => a.ProductRecipes)
                        .ThenInclude(a => a.Product)
                    .FirstOrDefaultAsync();
            }

            if (dailyRecipe != null)
            {
                model.RecipeOfTheDay = _mapper.Map<RecipeDisplayModel>(dailyRecipe);
            }

            model.RecipesCount = await _context.Recipes.CountAsync();
            model.FollowersCount = await _context.Users.CountAsync();

            var latestAndTopRatedRecipes = await _context.Recipes
                .Include(a => a.Ratings)
                .Include(a => a.User)
                .Include(a => a.ProductRecipes)
                    .ThenInclude(a => a.Product)
                .ToListAsync();

            model.LatestRecipes = _mapper.Map<IEnumerable<RecipeDisplayModel>>(
                latestAndTopRatedRecipes.OrderByDescending(a => a.PublishDate)).Take(3);

            model.TopRatedRecipes = _mapper.Map<IEnumerable<RecipeDisplayModel>>(
                latestAndTopRatedRecipes.OrderByDescending(a => a.AverageRating)).Take(3);

            return model;
        }

        public async Task<EditRecipeOutputModel> EditById(int id)
        {
            var recipe = await _context.Recipes
                .Include(a => a.ProductRecipes)
                .ThenInclude(a => a.Product)
                .FirstOrDefaultAsync(a => a.Id == id);

            ArgumentNullException.ThrowIfNull(recipe);
            ArgumentNullException.ThrowIfNull(recipe.ProductRecipes);

            var map = _mapper.Map<EditRecipeOutputModel>(recipe);
            map.Products = recipe.ProductRecipes
                .Select(a => new Tuple<string, string>(a.Product.Name, a.QuantityDetails));

            return map;
        }
    }
}