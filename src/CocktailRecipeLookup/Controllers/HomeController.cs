using Microsoft.AspNetCore.Mvc;

namespace CocktailRecipeLookup.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
