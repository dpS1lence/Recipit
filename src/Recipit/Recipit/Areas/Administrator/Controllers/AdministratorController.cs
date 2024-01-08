namespace Recipit.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Recipit.Contracts.Constants;

    [Authorize(Roles = RecipitRole.Administrator)]
    [Area(RecipitRole.Administrator)]
    public class AdministratorController : Controller
    { }
}