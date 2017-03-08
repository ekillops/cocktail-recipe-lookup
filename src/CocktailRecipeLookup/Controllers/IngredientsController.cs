using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CocktailRecipeLookup.Models;

namespace CocktailRecipeLookup.Controllers
{
    public class IngredientsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(string id)
        {
            Ingredient foundIngredient = IngredientModel.Details(id);
            return View(foundIngredient);
        }
    }
}
