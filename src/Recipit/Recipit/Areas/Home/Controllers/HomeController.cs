namespace Recipit.Areas.Home.Controllers
{
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Recipit.Middlewares;
    using Recipit.Models;
    using Recipit.Services.Recipes;
    using System.Diagnostics;

    [Area("Home")]
    public class HomeController(ILogger<HomeController> logger, IRecipeService recipeService) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IRecipeService _recipeService = recipeService;

        public async Task<IActionResult> Index() => View(await _recipeService.GetHomePage());

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature is not null && exceptionHandlerPathFeature.Error is not null)
            {
                _logger.LogError(exceptionHandlerPathFeature.Error, "Unhandled exception.");

                //TempData["messageDanger"] = exceptionHandlerPathFeature.Error.Message;
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
