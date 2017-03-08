using Microsoft.AspNetCore.Mvc;

namespace CocktailRecipeLookup.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search(string query, string searchType)
        {
            if(searchType == "ingredients")
            {
                return RedirectToAction("Search", "Ingredients", new { query = query });
            }
            else
            {
                return RedirectToAction("Search", "Drinks", new { query = query });
            }
        }
    }
}
