using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CocktailRecipeLookup.Controllers
{
    public class IngredientsController : Controller
    {        public IActionResult Index()
        {
            return View();
        }
    }
}
