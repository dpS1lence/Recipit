namespace Recipit.Areas.Home.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Models;
    using Recipit.Services.Recipes;
    using System.Diagnostics;

    [Area("Home")]
    public class HomeController(IRecipeService recipeService) : Controller
    {
        private readonly IRecipeService _recipeService = recipeService;

        public async Task<IActionResult> Index() => View(await _recipeService.GetHomePage());

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
