using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Recipit.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Administrator")]
    [Area("Administrator")]
    public class AdministratorController : Controller
    { }
}