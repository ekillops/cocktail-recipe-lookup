using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CocktailRecipeLookup.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CocktailRecipeLookup.Controllers
{
    public class DrinksController : Controller
    {
        public IActionResult Index()
        {
            if (IngredientModel.AllIngredients == null)
            {
                IngredientModel.GetAll();
            }
            return View(IngredientModel.AllIngredients);
        }

        public IActionResult Details(string id)
        {
            Drink foundDrink = DrinkModel.Details(id);
            return View(foundDrink);
        }

        public IActionResult Search(string query)
        {
            List<Drink> foundDrinks = DrinkModel.Search(query);
            return View("SearchResults", foundDrinks);
        }

        public IActionResult SearchByIngredients(List<string> ingredients)
        {
            List<Drink> foundDrinks = DrinkModel.FindDrinksWithIngredientsBroad(ingredients);
            return View("SearchResultsPartial", foundDrinks);
        }
    }
}
