namespace Recipit.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Services.Account;
    using Recipit.Services.Followers;

    [Route("followers")]
    public class FollowerController(IFollowerService followerService, IAccountService accountService) : AdministratorController
    {
        private readonly IFollowerService _followerService = followerService;
        private readonly IAccountService _accountService = accountService;

        [HttpGet]
        [Route("manage")]
        public async Task<IActionResult> All(int pageIndex = 1, int pageSize = 1) => View(await _followerService.GetAll(pageIndex, pageSize));

        [HttpDelete]
        public async Task Delete([FromQuery] string followerId) => await _accountService.DeleteUserById(followerId);
    }
}
