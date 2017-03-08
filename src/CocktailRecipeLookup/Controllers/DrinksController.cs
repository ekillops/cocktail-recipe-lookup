﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CocktailRecipeLookup.Models;

namespace CocktailRecipeLookup.Controllers
{
    public class DrinksController : Controller
    {
        public IActionResult UseAvailableIngredients()
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
            List<Drink> foundDrinks = DrinkModel.FindDrinksWithAvailable(ingredients);
            return View("SearchResultsPartial", foundDrinks);
        }
    }
}
