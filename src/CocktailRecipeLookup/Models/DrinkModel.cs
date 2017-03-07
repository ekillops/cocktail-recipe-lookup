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
        public static Drink Details(string id)
        {
            RestClient client = new RestClient("http://addb.absolutdrinks.com/");

            RestRequest request = new RestRequest("drinks/"+id+"/?apiKey=" + EnvironmentVariables.ADDbApiKey);

            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            DrinkQuery searchResults = JsonConvert.DeserializeObject<DrinkQuery>(response.Content);

            return searchResults.result[0];
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


            return removePartialMatches(resultDrinks.result, ingredients);
        }


        public static List<Drink> FindDrinksWithIngredientsBroad(List<string> ingredients)
        {
            List<Drink> bigDrinkList = new List<Drink>();

            string queryString = "drinks/with/";

            for(int i = 0; i < ingredients.Count; i++)
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

            return removePartialMatches(bigDrinkList, ingredients);
        }

        private static List<Drink> removePartialMatches(List<Drink> drinkList, List<string> userIngredients)
        {
            List<string> standardIngredients = new List<string> { "ice-cubes", "simple-syrup", "bitters", "lemon", "lime", "orange", "maraschino-berry", "apple", "soda-water" };

            List<Drink> matches = new List<Drink>();

            foreach (Drink drink in drinkList)
            {
                bool matching = true;
                foreach (DrinkIngredient ingredient in drink.ingredients)
                {
                    if (!userIngredients.Contains(ingredient.id) && !standardIngredients.Contains(ingredient.id))
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
