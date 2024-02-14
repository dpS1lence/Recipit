﻿namespace Recipit.Areas.Home.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Contracts.Constants;
    using Recipit.Infrastructure.Data;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Models.Account;
    using Recipit.Services.Account;
    using Recipit.Services.Followers;

    [AllowAnonymous]
    [Area("Home")]
    public class AccountController
        (UserManager<RecipitUser> userManager, 
        SignInManager<RecipitUser> signInManager, 
        IMapper mapper, 
        RecipitDbContext context, 
        IAccountService accountService, 
        RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly UserManager<RecipitUser> _userManager = userManager;
        private readonly SignInManager<RecipitUser> _signInManager = signInManager;
        private readonly IMapper _mapper = mapper;
        private readonly RecipitDbContext _context = context;
        private readonly IAccountService _accountService = accountService;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        [HttpGet("/login")]
        public IActionResult Login() => GetView();

        [HttpGet("/register")]
        public IActionResult Register() => GetView();

        [HttpGet("/u/{name}")]
        public async Task<IActionResult> Profile(string name) => View(await _accountService.GetByName(name));

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _mapper.Map<RecipitUser>(model);

            if (user == null || user.UserName == null || (await _userManager.FindByNameAsync(user.UserName)) != null)
            {
                return View(model);
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("User registration failed: {0}", string.Join(", ", result.Errors));

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
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
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
