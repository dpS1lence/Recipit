namespace Recipit.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ProductController : AdministratorController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
