using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CocktailRecipeLookup.Models;

namespace CocktailRecipeLookup.Controllers
{
    public class DrinksController : Controller
    {
        public IActionResult UseWhatYouHave()
        {
            if (IngredientModel.AllIngredients == null)
            {
                IngredientModel.GetAll();
            }
            return View(IngredientModel.AllIngredients);
        }

        public IActionResult CustomSearch()
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
            ViewBag.DrinkIngredients = DrinkModel.GetDrinkIngredients(id);
            return View(foundDrink);
        }

        public IActionResult Search(string query)
        {
            List<Drink> foundDrinks = DrinkModel.Search(query);
            return View("SearchResults", foundDrinks);
        }

        public IActionResult SearchByIngredients(List<string> ingredients)
        {
            List<Drink> foundDrinks = DrinkModel.FindDrinksWithAvailableIngredients(ingredients);
            return View("SearchResultsPartial", foundDrinks);
        }

        public IActionResult SearchCustom(List<string> ingredients, string searchType)
        {
            List<Drink> foundDrinks = new List<Drink>();

            switch (searchType)
            {
                case "exactMatch":
                    foundDrinks = DrinkModel.FindDrinksWithExactIngredients(ingredients);
                    break;
                case "allContaining":
                    foundDrinks = DrinkModel.FindAllDrinksContainingIngredients(ingredients);
                    break;
                case "whatYouHave":
                    foundDrinks = DrinkModel.FindDrinksWithAvailableIngredients(ingredients);
                    break;
                default:
                    break;
            }

            return View("SearchResultsPartial", foundDrinks);
        }
    }
}
