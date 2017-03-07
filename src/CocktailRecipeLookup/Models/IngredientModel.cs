using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailRecipeLookup.Models
{
    public static class IngredientModel
    {
        public static List<Ingredient> AllIngredients { get; set; }

        public static void GetAll()
        {
            List<Ingredient> allIngredients = new List<Ingredient>();

            RestClient client = new RestClient("http://addb.absolutdrinks.com/");
            RestRequest request = new RestRequest("ingredients/?apiKey=" + EnvironmentVariables.ADDbApiKey + "&pageSize=100");
            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            IngredientQuery resultIngredients = JsonConvert.DeserializeObject<IngredientQuery>(response.Content);
            allIngredients.AddRange(resultIngredients.result);

            while (allIngredients.Count < resultIngredients.totalResult - 1)
            {
                List<Ingredient> nextPage = GetNextPageIngredients(allIngredients.Count);
                allIngredients.AddRange(nextPage);
            }

            AllIngredients = allIngredients;
        }

        private static List<Ingredient> GetNextPageIngredients(int retrievedSoFar)
        {
            RestClient client = new RestClient("http://addb.absolutdrinks.com/");
            RestRequest request = new RestRequest("ingredients/?apiKey=" + EnvironmentVariables.ADDbApiKey + "&pageSize=100&start=" + (retrievedSoFar + 1));

            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            IngredientQuery resultIngredients = JsonConvert.DeserializeObject<IngredientQuery>(response.Content);
            return resultIngredients.result;
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
