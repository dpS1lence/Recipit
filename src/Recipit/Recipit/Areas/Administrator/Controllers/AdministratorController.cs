using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Recipit.Areas.Administrator.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    { }
}
