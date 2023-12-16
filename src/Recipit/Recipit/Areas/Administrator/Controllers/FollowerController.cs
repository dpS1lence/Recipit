namespace Recipit.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Services.Followers;

    public class FollowerController(IFollowerService followerService) : AdministratorController
    {
        private readonly IFollowerService _followerService = followerService;

        [HttpGet]
        [Route("followers/manage")]
        public async Task<IActionResult> All() => View(await _followerService.GetAll());

        [HttpPost]
        public async Task Delete([FromBody] string followerId) => await _followerService.Delete(followerId);
    }
}
