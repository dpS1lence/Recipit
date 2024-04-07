namespace Recipit.Areas.Home.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Contracts.Constants;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Infrastructure.Extensions.Contracts;
    using Recipit.Models.Account;
    using Recipit.Services.Account;
    using System.Security.Claims;

    [AllowAnonymous]
    [Area("Home")]
    public class AccountController
        (UserManager<RecipitUser> userManager,
        SignInManager<RecipitUser> signInManager,
        IMapper mapper,
        RecipitDbContext context,
        IAccountService accountService,
        IConfiguration configuration,
        RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly UserManager<RecipitUser> _userManager = userManager;
        private readonly SignInManager<RecipitUser> _signInManager = signInManager;
        private readonly IMapper _mapper = mapper;
        private readonly RecipitDbContext _context = context;
        private readonly IAccountService _accountService = accountService;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public IConfiguration Configuration => configuration;

        [HttpGet("/login")]
        public IActionResult Login() => GetView();

        [HttpGet("/register")]
        public IActionResult Register() => GetView();

        [HttpGet("/u/{name}")]
        public async Task<IActionResult> Profile(string name)
        {
            if (string.Equals(User?.FindFirst(ClaimTypes.Name)?.Value, name, StringComparison.OrdinalIgnoreCase))
            {
                return Redirect("/profile");
            }

            return View(await _accountService.GetByName(name));
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _mapper.Map<RecipitUser>(model);

            if (user is null || user.UserName is null || (await _userManager.FindByNameAsync(user.UserName)) is not null)
            {
                TempData["messageDanger"] = "Профил с това потребителско име вече съществува!";

                return View(model);
            }

            user.Photo = Configuration.GetSection("UserSettings").Get<UserSettings>()!.Photo;

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("User registration failed: {0}", string.Join(", ", result.Errors));

                TempData["messageDanger"] = "Вашата регистрация беше неуспешна!";

                return View(model);
            }

            if (!await _roleManager.RoleExistsAsync(RecipitRole.Follower))
            {
                var role = new IdentityRole(RecipitRole.Follower);
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, RecipitRole.Follower);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var existingClaims = await _userManager.GetClaimsAsync(user);
                var profilePictureClaim = existingClaims.FirstOrDefault(c => c.Type == "profile_picture_url");
                if (profilePictureClaim != null)
                {
                    await _userManager.RemoveClaimAsync(user, profilePictureClaim);
                }

                if (!string.IsNullOrEmpty(user.Photo))
                {
                    var claim = new Claim("profile_picture_url", user.Photo);
                    await _userManager.AddClaimAsync(user, claim);
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    TempData["message"] = $"Добре дошъл {user.FirstName}!";

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid Login");

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        private IActionResult GetView()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                if (User.IsInRole(RecipitRole.Administrator))
                {
                    return RedirectToAction("Followers", "Home", new { Area = "Administrator" });
                }
                if (User.IsInRole(RecipitRole.Follower))
                {
                    return RedirectToAction("Index", "Home", new { Area = "Follower" });
                }
            }

            return View();
        }
    }
}
