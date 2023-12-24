using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Recipit.Areas.Follower.Controllers
{
    [Authorize(Roles = "Follower")]
    public class FollowerController : Controller
    { }
}
