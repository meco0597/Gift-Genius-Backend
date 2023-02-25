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

        public async Task<Dictionary<string, Task<AmazonProductResponseModel>>> GetAmazonProductDetailsFromListOfQueries(List<string> queries)
        {
            Dictionary<string, Task<AmazonProductResponseModel>> queryToProduct = new Dictionary<string, Task<AmazonProductResponseModel>>();
            foreach (string query in queries)
            {
                var task = this.GetAmazonChoiceProductDetails(query);
                queryToProduct.Add(query, task);
            }

            await Task.WhenAll(queryToProduct.Values.ToArray());
            return queryToProduct;
        }

        public async Task<AmazonProductResponseModel> GetAmazonChoiceProductDetails(string productQuery)
        {
            var amazonResponseString = await this.SendApiRequest(productQuery);
            var productList = JsonSerializer.Deserialize<AmazonProductSearchResponseModel>(amazonResponseString);

            var amazonChoice = productList.Result.FirstOrDefault(x => x.AmazonChoice == true);
            if (amazonChoice == null)
            {
                return productList.Result.MinBy(x => x.Position.Position);
            }

            return amazonChoice;
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