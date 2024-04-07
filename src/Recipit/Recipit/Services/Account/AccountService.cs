using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Polly;
using Recipit.Infrastructure.Data.Models;
using Recipit.Infrastructure.Data;
using Recipit.Infrastructure.Extensions.Contracts;
using Recipit.ViewModels.Account;
using System.Configuration;
using Recipit.Services.Followers;
using Microsoft.EntityFrameworkCore;
using Recipit.Pagination;
using Recipit.ViewModels.Recipe;
using Recipit.Contracts.Helpers;
using Recipit.Contracts;
using Recipit.Services.Images;

namespace Recipit.Services.Account
{
    public class AccountService
        (RecipitDbContext context,
        UserManager<Comment> userManager,
        ILogger<FollowerService> logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        HttpClient client)
        : IAccountService
    {
        private readonly RecipitDbContext _context = context;
        private readonly UserManager<Comment> _userManager = userManager;
        private readonly ILogger _logger = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMapper _mapper = mapper;
        private readonly HttpClient _client = client;

        public async Task ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userManager.FindByIdAsync(GetUser.Id(_httpContextAccessor))
                ?? throw new ArgumentNullException(nameof(model));

            if (model.NewPassword == model.ConfirmNewPassword)
                await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        }

        public async Task DeleteProfile() =>
            await DeleteUserById(GetUser.Id(_httpContextAccessor));

        public async Task<string> DeleteUserById(string uId)
        {
            var user = await _userManager.FindByIdAsync(uId)
                ?? throw new ArgumentNullException(nameof(uId));

            var userRecipes = await _context.Recipes
                    .Where(r => r.UserId == uId)
                    .ToListAsync();
            var recipeComments = await _context.Comments
                    .Where(c => c.UserId == uId || _context.Recipes.Any(r => r.UserId == uId && r.Id == c.RecipeId))
                    .ToListAsync();
            var recipeProducts = await _context.ProductsRecipies
                    .Where(pr => _context.Recipes.Any(r => r.UserId == uId && r.Id == pr.RecipeId))
                    .ToListAsync();
            var ratings = await _context.Ratings
                    .Where(r => r.UserId == uId)
                    .ToListAsync();

            _context.RemoveRange(userRecipes);
            _context.RemoveRange(recipeComments);
            _context.RemoveRange(recipeProducts);
            _context.RemoveRange(ratings);

            await _context.SaveChangesAsync();

            await _userManager.DeleteAsync(user);

            return user.UserName!;
        }

        public async Task EditProfile(EditProfileInputModel model)
        {
            Validate.Model(model, _logger);

            var user = await _userManager.FindByIdAsync(GetUser.Id(_httpContextAccessor))
                        ?? throw new ArgumentNullException(nameof(model));

            bool isUpdated = false;

            async Task updateEmailAsync(string newEmail)
            {
                var emailToken = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
                var result = await _userManager.ChangeEmailAsync(user, newEmail, emailToken);

                if (result.Succeeded)
                    isUpdated = true;
                else
                    _logger.LogError("Failed to update email for user {UserId}.", GetUser.Id(_httpContextAccessor));
            }

            if (!string.IsNullOrEmpty(model.Email) && user.Email != model.Email)
                await updateEmailAsync(model.Email);

            void updateProperty(string newValue, Action<string> assignAction)
            {
                if (!string.IsNullOrEmpty(newValue))
                {
                    assignAction(newValue);
                    isUpdated = true;
                }
            }

            updateProperty(model.FirstName, newValue => user.FirstName = newValue);
            updateProperty(model.LastName, newValue => user.LastName = newValue);

            if (model.Photo != null && model.Photo != default)
            {
                user.Photo = await UploadImage.ToImgur(model.Photo, _client);
                isUpdated = true;
            }

            if (isUpdated)
                await _userManager.UpdateAsync(user);
        }

        public async Task<UserViewModel> GetByName(string name)
        {
            var user = await _userManager.FindByNameAsync(name)
                ?? throw new ArgumentNullException(nameof(name));

            var map = _mapper.Map<UserViewModel>(user);
            var recipes = await _context.Recipes
                .Where(a => a.UserId == user.Id)
                .Include(a => a.Comments)
                    .ThenInclude(a => a.User)
                .Include(a => a.ProductRecipes)
                    .ThenInclude(a => a.Product)
                .Include(a => a.Ratings)
                .ToListAsync();

            map.UserRecipes = _mapper.Map<IEnumerable<RecipeDisplayModel>>(recipes);

            _logger.LogInformation(nameof(user));

            return map;
        }

        public async Task<UserViewModel> GetCurrentUser()
        {
            var user = await GetUser.Data(_userManager, _httpContextAccessor);

            return await GetByName(user?.UserName
                ?? throw new ArgumentNullException(nameof(user)));
        }
    }
}
