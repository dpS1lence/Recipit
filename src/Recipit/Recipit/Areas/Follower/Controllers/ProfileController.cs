namespace Recipit.Areas.Follower.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Contracts.Constants;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Services.Account;
    using Recipit.ViewModels.Account;

    public class ProfileController(IAccountService accountService, SignInManager<Infrastructure.Data.Models.Comment> signInManager) : FollowerController
    {
        private readonly SignInManager<Infrastructure.Data.Models.Comment> _signInManager = signInManager;
        private readonly IAccountService _accountService = accountService;

        [HttpGet("/profile")]
        public async Task<IActionResult> Profile() => View(await _accountService.GetCurrentUser());

        [HttpPut]
        public async Task Edit([FromForm] EditProfileInputModel model)
        {
            TempData["message"] = "Успешно редактирахте своя профил!";

            await _accountService.EditProfile(model);
        }

        [HttpGet]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            await _accountService.ChangePassword(model);

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home", new { Area = "Home" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            if (!User.IsInRole(RecipitRole.Administrator))
            {
                await _accountService.DeleteProfile();

                await _signInManager.SignOutAsync();

                return RedirectToAction("Index", "Home", new { Area = "Home" });
            }

            return RedirectToAction(nameof(Profile));
        }
    }
}
