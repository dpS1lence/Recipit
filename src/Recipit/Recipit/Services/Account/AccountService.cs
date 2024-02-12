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

namespace Recipit.Services.Account
{
    public class AccountService
        (RecipitDbContext context, UserManager<RecipitUser> userManager, ILogger<FollowerService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : IAccountService
    {
        private readonly RecipitDbContext _context = context;
        private readonly UserManager<RecipitUser> _userManager = userManager;
        private readonly ILogger _logger = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMapper _mapper = mapper;

        public async Task<UserViewModel> GetByName(string name)
        {
            var user = await _userManager.FindByNameAsync(name) ?? throw new ArgumentNullException(nameof(name));
            var map = _mapper.Map<UserViewModel>(user);
            var recipes = await _context.Recipes.Where(a => a.UserId == user.Id).Include(a => a.Comments).Include(a => a.ProductRecipes).ThenInclude(a => a.Product).ToListAsync();
            map.UserRecipes = _mapper.Map<IEnumerable<RecipeDisplayModel>>(recipes);

            _logger.LogInformation(nameof(user));

            return map;
        }

        public async Task<UserViewModel> GetCurrentUser()
        {
            var user = await _userManager.FindByIdAsync(GetUser.Id(_httpContextAccessor));

            return await GetByName(user?.UserName ?? throw new ArgumentNullException(nameof(user)));
        }
    }
}
