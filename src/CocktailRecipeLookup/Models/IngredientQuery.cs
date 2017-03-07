using System.Collections.Generic;

namespace CocktailRecipeLookup.Models
{
    public class Ingredient
    {
        public string description { get; set; }
        public bool isCarbonated { get; set; }
        public bool isAlcoholic { get; set; }
        public bool isBaseSpirit { get; set; }
        public bool isJuice { get; set; }
        public string type { get; set; }
        public string languageBranch { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }

    public class IngredientQuery
    {
        public List<Ingredient> result { get; set; }
        public int totalResult { get; set; }
        public string next { get; set; }
    }
}
