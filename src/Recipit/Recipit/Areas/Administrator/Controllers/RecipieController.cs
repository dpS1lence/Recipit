namespace Recipit.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class RecipieController : AdministratorController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
