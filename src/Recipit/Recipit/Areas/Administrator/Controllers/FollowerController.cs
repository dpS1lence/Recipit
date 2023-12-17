namespace Recipit.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Services.Followers;

    [Route("followers")]
    public class FollowerController(IFollowerService followerService) : AdministratorController
    {
        private readonly IFollowerService _followerService = followerService;

        [HttpGet]
        [Route("manage")]
        public async Task<IActionResult> All(int pageIndex, int pageSize) => View(await _followerService.GetAll(pageIndex, pageSize));

        [HttpPost]
        public async Task Delete([FromBody] string followerId) => await _followerService.Delete(followerId);
    }
}
