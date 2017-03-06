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
        public static List<Drink> FindDrinksWithIngredients(List<string> ingredients)
        {

            // Construct api query from ingredient list
            string drinkQuery = "drinks/with/";

            for(int i = 0; i < ingredients.Count; i++)
            {
                if(i != ingredients.Count - 1)
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
            RestRequest request = new RestRequest(drinkQuery + "?apiKey=" +EnvironmentVariables.ADDbApiKey);

            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            DrinkQuery resultDrinks = JsonConvert.DeserializeObject<DrinkQuery>(response.Content);

            return removePartialMatches(resultDrinks, ingredients);
        }

        private static List<Drink> removePartialMatches(DrinkQuery drinkQuery, List<string> ingredients)
        {
            List<string> standardIngredients = new List<string> { "ice-cubes", "simple-syrup", "bitters", "lemon", "lime", "orange", "maraschino-berry", "apple" };
            //List<string> garnishIngredients = new List<string> { "lemon", "lime", "orange", "maraschino-berry", "apple" };

            List<Drink> matches = new List<Drink>();

            foreach (Drink drink in drinkQuery.result)
            {
                bool matching = true;
                foreach(Ingredient ingredient in drink.ingredients)
                {
                    if(!ingredients.Contains(ingredient.id) && !standardIngredients.Contains(ingredient.id))
                    {
                        matching = false;
                        break;
                    }
                }
                if(matching)
                {
                    matches.Add(drink);
                }
            }
            return matches;
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
