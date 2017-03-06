using System.Collections.Generic;

namespace CocktailRecipeLookup.Models
{
    // Generated classes from ADDb json response to Drink query
    public class Skill
    {
        public string id { get; set; }
        public string name { get; set; }
        public int value { get; set; }
    }

    public class Video
    {
        public string video { get; set; }
        public string type { get; set; }
    }

    public class ServedIn
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class Ingredient
    {
        public string type { get; set; }
        public string id { get; set; }
        public string text { get; set; }
        public string textPlain { get; set; }
    }

    public class Taste
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class Occasion
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class Tool
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class Action
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class Result // Drink
    {
        public string description { get; set; }
        public string story { get; set; }
        public string color { get; set; }
        public int rating { get; set; }
        public Skill skill { get; set; }
        public List<Video> videos { get; set; }
        public bool isAlcoholic { get; set; }
        public bool isCarbonated { get; set; }
        public bool isHot { get; set; }
        public List<object> tags { get; set; }
        public ServedIn servedIn { get; set; }
        public List<Ingredient> ingredients { get; set; }
        public List<Taste> tastes { get; set; }
        public List<Occasion> occasions { get; set; }
        public List<Tool> tools { get; set; }
        public List<object> drinkTypes { get; set; }
        public List<Action> actions { get; set; }
        public List<string> brands { get; set; }
        public string languageBranch { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string descriptionPlain { get; set; }
    }

    public class DrinkQuery
    {
        public List<Result> result { get; set; }
        public int totalResult { get; set; }
        public string next { get; set; }
    }
}
