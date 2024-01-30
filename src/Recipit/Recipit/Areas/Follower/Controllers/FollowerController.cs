namespace Recipit.Areas.Follower.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Contracts.Constants;

    [Area(RecipitRole.Follower)]
    [Authorize]
    public class FollowerController : Controller
    { }
}
