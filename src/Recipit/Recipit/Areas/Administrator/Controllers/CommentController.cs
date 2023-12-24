namespace Recipit.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class CommentController : AdministratorController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
