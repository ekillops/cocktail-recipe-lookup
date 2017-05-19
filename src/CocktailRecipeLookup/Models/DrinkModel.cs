using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

namespace CocktailRecipeLookup.Models
{
    public class DrinkModel
    {
        public static List<Drink> Search(string query)
        {
            RestClient client = new RestClient("http://addb.absolutdrinks.com/");
            RestRequest request = new RestRequest("quickSearch/drinks/" + query + "/?apiKey=" + EnvironmentVariables.ADDbApiKey);
            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            DrinkQuery searchResults = JsonConvert.DeserializeObject<DrinkQuery>(response.Content);
            return searchResults.result;
        }

        public static Drink Details(string id)
        {
            RestClient client = new RestClient("http://addb.absolutdrinks.com/");

            RestRequest request = new RestRequest("drinks/" + id + "/?apiKey=" + EnvironmentVariables.ADDbApiKey);

            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            DrinkQuery searchResults = JsonConvert.DeserializeObject<DrinkQuery>(response.Content);

            return searchResults.result[0];
        }

        public static Dictionary<string, Ingredient> GetDrinkIngredients(string id)
        {
            Drink foundDrink = Details(id);

            Dictionary<string, Ingredient> drinkIngredients = new Dictionary<string, Ingredient>();
            foreach (DrinkIngredient ingredient in foundDrink.ingredients)
            {
                RestClient client = new RestClient("http://addb.absolutdrinks.com/");

                RestRequest request = new RestRequest("ingredients/" + ingredient.id + "/?apiKey=" + EnvironmentVariables.ADDbApiKey);
                RestResponse response = new RestResponse();

                Task.Run(async () =>
                {
                    response = await GetResponseContentAsync(client, request) as RestResponse;
                }).Wait();

                Ingredient foundIngredient = JsonConvert.DeserializeObject<Ingredient>(response.Content);

                drinkIngredients.Add(foundIngredient.id, foundIngredient);
            }
            return drinkIngredients;
        }

        public static List<Drink> FindDrinksWithExactIngredients(List<string> ingredients)
        {

            // Construct api query from ingredient list
            string drinkQuery = "drinks/with/";

            for (int i = 0; i < ingredients.Count; i++)
            {
                if (i != ingredients.Count - 1)
                {
                    drinkQuery += ingredients[i] + "/and/";
                }
                else
                {
                    drinkQuery += ingredients[i];
                }
            }

            // Plug query into RestSharp
            RestClient client = new RestClient("http://addb.absolutdrinks.com/");

            RestRequest request = new RestRequest(drinkQuery + "?apiKey=" + EnvironmentVariables.ADDbApiKey);

            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            DrinkQuery resultDrinks = JsonConvert.DeserializeObject<DrinkQuery>(response.Content);


            return RemovePartialMatches(resultDrinks.result, ingredients);
        }

        public static List<Drink> FindDrinksWithAvailableIngredients(List<string> ingredients)
        {
            ingredients = AddRelatedIngredients(ingredients);

            List<Drink> bigDrinkList = new List<Drink>();

            string queryString = "drinks/with/";

            for (int i = 0; i < ingredients.Count; i++)
            {
                if (i != ingredients.Count - 1)
                {
                    queryString += ingredients[i] + "/or/";
                }
                else
                {
                    queryString += ingredients[i] + "/?apiKey=" + EnvironmentVariables.ADDbApiKey + "&pageSize=100";
                }
            }

            RestClient client = new RestClient("http://addb.absolutdrinks.com/");
            RestRequest request = new RestRequest(queryString);
            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            DrinkQuery resultDrinks = JsonConvert.DeserializeObject<DrinkQuery>(response.Content);

            bigDrinkList.AddRange(resultDrinks.result);

            while (bigDrinkList.Count < resultDrinks.totalResult - 1)
            {
                List<Drink> nextPage = GetNextPageDrinks(bigDrinkList.Count, queryString);
                bigDrinkList.AddRange(nextPage);
            }

            return RemovePartialMatches(bigDrinkList, ingredients);
        }

        public static List<Drink> FindDrinksContainingAllIngredients(List<string> ingredients)
        {
            List<Drink> bigDrinkList = new List<Drink>();

            string queryString = "drinks/with/";

            for (int i = 0; i < ingredients.Count; i++)
            {
                if (i != ingredients.Count - 1)
                {
                    queryString += ingredients[i] + "/and/";
                }
                else
                {
                    queryString += ingredients[i] + "/?apiKey=" + EnvironmentVariables.ADDbApiKey + "&pageSize=100";
                }
            }

            RestClient client = new RestClient("http://addb.absolutdrinks.com/");
            RestRequest request = new RestRequest(queryString);
            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            DrinkQuery resultDrinks = JsonConvert.DeserializeObject<DrinkQuery>(response.Content);

            bigDrinkList.AddRange(resultDrinks.result);

            while (bigDrinkList.Count < resultDrinks.totalResult - 1)
            {
                List<Drink> nextPage = GetNextPageDrinks(bigDrinkList.Count, queryString);
                bigDrinkList.AddRange(nextPage);
            }

            return bigDrinkList;
        }

        public static List<Drink> FindDrinksContainingAnyIngredients(List<string> ingredients)
        {
            ingredients = AddRelatedIngredients(ingredients);

            List<Drink> bigDrinkList = new List<Drink>();

            string queryString = "drinks/with/";

            for (int i = 0; i < ingredients.Count; i++)
            {
                if (i != ingredients.Count - 1)
                {
                    queryString += ingredients[i] + "/or/";
                }
                else
                {
                    queryString += ingredients[i] + "/?apiKey=" + EnvironmentVariables.ADDbApiKey + "&pageSize=100";
                }
            }

            RestClient client = new RestClient("http://addb.absolutdrinks.com/");
            RestRequest request = new RestRequest(queryString);
            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            DrinkQuery resultDrinks = JsonConvert.DeserializeObject<DrinkQuery>(response.Content);

            bigDrinkList.AddRange(resultDrinks.result);

            while (bigDrinkList.Count < resultDrinks.totalResult - 1)
            {
                List<Drink> nextPage = GetNextPageDrinks(bigDrinkList.Count, queryString);
                bigDrinkList.AddRange(nextPage);
            }

            return bigDrinkList;
        }

        // Filter function
        private static List<Drink> RemovePartialMatches(List<Drink> drinkList, List<string> userIngredients)
        {

            // List of ingredients to ignore when filtering out partial matches
            List<string> standardIngredients = new List<string> { "ice-cubes", "simple-syrup", "sugar-cube-white", "sugar-superfine", "bitters", "lemon", "lime", "orange", "maraschino-berry", "apple", "soda-water", "egg-white" };

            List<Drink> matches = new List<Drink>();

            foreach (Drink drink in drinkList)
            {
                bool matching = true;
                foreach (DrinkIngredient ingredient in drink.ingredients)
                {
                    //if (!userIngredients.Contains(ingredient.id) && !standardIngredients.Contains(ingredient.id))
                    if (!userIngredients.Contains(ingredient.id))
                    {
                        matching = false;
                        break;
                    }
                }
                if (matching && !matches.Contains(drink))
                {
                    matches.Add(drink);
                }
            }
            return matches;
        }

        private static List<string> AddRelatedIngredients(List<string> ingredientList)
        {
            // Triple Sec
            if (ingredientList.Contains("triple-sec"))
            {
                ingredientList.Add("cointreau");
            }
            else if (ingredientList.Contains("cointreau"))
            {
                ingredientList.Add("triple-sec");
            }

            // Brandy
            if (ingredientList.Contains("brandy"))
            {
                ingredientList.Add("cognac");
            }
            else if (ingredientList.Contains("cognac"))
            {
                ingredientList.Add("brandy");
            }

            // Absinthe
            if (ingredientList.Contains("absinthe"))
            {
                ingredientList.Add("pastis");
            }
            else if (ingredientList.Contains("pastis"))
            {
                ingredientList.Add("absinthe");
            }

            // Rye Whiskey
            if (ingredientList.Contains("rye-whiskey"))
            {
                ingredientList.Add("canadian-whisky");
                ingredientList.Add("bourbon");
            }
            else if (ingredientList.Contains("canadian-whisky"))
            {
                ingredientList.Add("rye-whiskey");
                ingredientList.Add("bourbon");
            }

            // Cassis
            if (ingredientList.Contains("creme-de-cassis"))
            {
                ingredientList.Add("black-currant-liqueur");
            }
            else if (ingredientList.Contains("black-currant-liqueur"))
            {
                ingredientList.Add("creme-de-cassis");
            }

            // Sweet Vermouth
            if (ingredientList.Contains("vermouth-sweet"))
            {
                ingredientList.Add("red-vermouth");
                ingredientList.Add("italian-vermouth");
            }
            else if (ingredientList.Contains("red-vermouth"))
            {
                ingredientList.Add("vermouth-sweet");
                ingredientList.Add("italian-vermouth");
            }
            else if (ingredientList.Contains("italian-vermouth"))
            {
                ingredientList.Add("vermouth-sweet");
                ingredientList.Add("red-vermouth");
            }

            // Ginger Beer
            if (ingredientList.Contains("ginger-beer"))
            {
                ingredientList.Add("ginger-ale");
            }
            else if (ingredientList.Contains("ginger-ale"))
            {
                ingredientList.Add("ginger-beer");
            }

            // Egg
            if (ingredientList.Contains("egg-yolk-and-white"))
            {
                ingredientList.Add("egg-white");
                ingredientList.Add("egg-yolk");
            }

            // Sparkling Wine
            if (ingredientList.Contains("sparkling-wine"))
            {
                ingredientList.Add("champagne");
                ingredientList.Add("prosecco");
            }
            else if (ingredientList.Contains("prosecco"))
            {
                ingredientList.Add("champagne");
                ingredientList.Add("sparkling-wine");
            }
            else if (ingredientList.Contains("champagne"))
            {
                ingredientList.Add("prosecco");
                ingredientList.Add("sparkling-wine");
            }

            // Hot & Cold
            if (ingredientList.Contains("cranberry-juice"))
            {
                ingredientList.Add("cranberry-juice-hot");
            }
            else if (ingredientList.Contains("cranberry-juice-hot"))
            {
                ingredientList.Add("cranberry-juice-hot");
            }

            if (ingredientList.Contains("cider"))
            {
                ingredientList.Add("cider-hot");
            }
            else if (ingredientList.Contains("cider-hot"))
            {
                ingredientList.Add("cider-hot");
            }

            if (ingredientList.Contains("tea"))
            {
                ingredientList.Add("tea-cold");
            }
            else if (ingredientList.Contains("tea-cold"))
            {
                ingredientList.Add("tea");
            }

            if (ingredientList.Contains("coffee"))
            {
                ingredientList.Add("coffee-cold");
            }
            else if (ingredientList.Contains("coffee-cold"))
            {
                ingredientList.Add("coffee");
            }

            if (ingredientList.Contains("espresso-coffee"))
            {
                ingredientList.Add("espresso-coffee-cold");
            }
            else if (ingredientList.Contains("espresso-coffee-cold"))
            {
                ingredientList.Add("espresso-coffee");
            }



            return ingredientList;
        }

        private static List<Drink> GetNextPageDrinks(int retrievedSoFar, string query)
        {
            RestClient client = new RestClient("http://addb.absolutdrinks.com/");
            RestRequest request = new RestRequest(query + "&start=" + (retrievedSoFar + 1));

            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            DrinkQuery resultDrinks = JsonConvert.DeserializeObject<DrinkQuery>(response.Content);
            return resultDrinks.result;
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            TaskCompletionSource<IRestResponse> tcs = new TaskCompletionSource<IRestResponse>();

            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
