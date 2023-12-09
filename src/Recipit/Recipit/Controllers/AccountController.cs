using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipit.Infrastructure.Data;
using Recipit.Infrastructure.Data.Models;
using Recipit.Models.Account;

namespace Recipit.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<RecipitUser> _userManager;
        private readonly SignInManager<RecipitUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly RecipitDbContext _context;

        public AccountController(UserManager<RecipitUser> userManager, SignInManager<RecipitUser> signInManager, IMapper mapper, RecipitDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _mapper.Map<RecipitUser>(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(model);
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

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}
