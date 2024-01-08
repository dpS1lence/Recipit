namespace Recipit.Areas.Follower.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Contracts.Constants;


    [Authorize(Roles = RecipitRole.Follower)]
    public class FollowerController : Controller
    { }
}
