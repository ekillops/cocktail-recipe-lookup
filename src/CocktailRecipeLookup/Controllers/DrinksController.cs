using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CocktailRecipeLookup.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CocktailRecipeLookup.Controllers
{
    public class DrinksController : Controller
    {
        public DrinkQuery SearchByIngredients(List<string> ingredients)
        {
            DrinkQuery foundDrinks = Drink.FindDrinksWithIngredients(ingredients);
            return foundDrinks;
        }
    }
}


//new List<string> { "lemon-juice", "gin", "triple-sec" }