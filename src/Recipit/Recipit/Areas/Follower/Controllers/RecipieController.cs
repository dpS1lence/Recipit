namespace Recipit.Areas.Follower.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class RecipieController : FollowerController
    {
        public IActionResult Create() => View();
    }
}
