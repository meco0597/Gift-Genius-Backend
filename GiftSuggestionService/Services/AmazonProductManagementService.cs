using System;
using System.Collections.Generic;
using System.Net.Http;
using GiftSuggestionService.Models;
using GiftSuggestionService.Configurations;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;

namespace GiftSuggestionService.Services
{
    public class AmazonProductManagementService
    {
        private readonly string rapidApiKey;
        private readonly AmazonProductConfiguration amazonProductConfiguration;

        public AmazonProductManagementService(
            KeyvaultAccessorService keyvaultAccessorService,
            IOptionsMonitor<AmazonProductConfiguration> optionsMonitor)
        {
            this.amazonProductConfiguration = optionsMonitor.CurrentValue;
            this.rapidApiKey = keyvaultAccessorService.GetSecretAsync(this.amazonProductConfiguration.SecretName).Result;
        }

        public async Task<Dictionary<string, Task<List<AmazonProductResponseModel>>>> GetAmazonProductDetailsFromListOfQueries(List<string> queries, int? nullableMaxPrice, int numOfProducts = 5)
        {
            int maxPrice = nullableMaxPrice ?? 50;
            Dictionary<string, Task<List<AmazonProductResponseModel>>> queryToProduct = new Dictionary<string, Task<List<AmazonProductResponseModel>>>();

            foreach (string query in queries)
            {
                try
                {
                    var task = this.ChooseAmazonProducts(query, maxPrice, numOfProducts);
                    queryToProduct.Add(query, task);
                }
                catch (ArgumentException)
                {
                    continue;
                }
            }

            await Task.WhenAll(queryToProduct.Values.ToArray());
            return queryToProduct;
        }

        public async Task<List<AmazonProductResponseModel>> ChooseAmazonProducts(string productQuery, int maxPrice, int numOfProducts)
        {
            List<AmazonProductResponseModel> productsToReturn = new List<AmazonProductResponseModel>();

            // search amazon for the query
            var amazonResponseString = await this.SendApiRequest(productQuery);

            var responseModel = JsonSerializer.Deserialize<AmazonProductSearchResponseModel>(amazonResponseString);
            var productList = responseModel.Result;

            // Filter by max price
            productList = productList.Where(x => x.Price.CurrentPrice <= maxPrice).ToList();

            // add the amazon choice if present
            var amazonChoice = productList.FirstOrDefault(x => x.AmazonChoice == true);
            if (amazonChoice != null)
            {
                productsToReturn.Add(amazonChoice);
            }

            // take the numOfProducts lowest scores
            productList = productList.OrderBy(x => Double.Parse(x.Score)).ToList();
            return productList.Take(numOfProducts - productsToReturn.Count).ToList();
        }

        public async Task<string> SendApiRequest(string query)
        {
            var queryParam = this.NormalizeQueryIntoQueryParams(query);
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://amazon23.p.rapidapi.com/product-search?query={queryParam}&country=US"),
                    Headers =
                    {
                        { "X-RapidAPI-Key", this.rapidApiKey },
                        { "X-RapidAPI-Host", this.amazonProductConfiguration.ApiUrl },
                    },
                };
                using (var response = await client.SendAsync(request))
                {
                    if (response.StatusCode.Equals(System.Net.HttpStatusCode.UnprocessableEntity))
                    {
                        throw new ArgumentException($"Amazon didn't like this query: {query}");
                    }

                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);
                    return body;
                }
            }
        }

        private string NormalizeQueryIntoQueryParams(string query)
        {
            return query.Trim().Replace(" ", "%20");
        }
    }
}