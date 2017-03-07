using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CocktailRecipeLookup.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CocktailRecipeLookup.Controllers
{
    public class DrinksController : Controller
    {
        public List<Drink> SearchByIngredients(List<string> ingredients)
        {
            List<Drink> foundDrinks = DrinkModel.FindDrinksWithIngredientsBroad(ingredients);
            return foundDrinks;
        }
    }
}
