using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Net.Http;
using GiftSuggestionService.Models;
using GiftSuggestionService.Configurations;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;
using Polly;
using Polly.Retry;
using System.Collections.Concurrent;

namespace GiftSuggestionService.Services
{
    public class AmazonProductManagementService
    {
        private readonly string rapidApiKey;
        private readonly AmazonProductConfiguration amazonProductConfiguration;
        private string amazonAffiliateTag = "givr02-20";

        public AmazonProductManagementService(
            KeyvaultAccessorService keyvaultAccessorService,
            IOptionsMonitor<AmazonProductConfiguration> optionsMonitor)
        {
            this.amazonProductConfiguration = optionsMonitor.CurrentValue;
            this.rapidApiKey = keyvaultAccessorService.GetSecretAsync(this.amazonProductConfiguration.SecretName).Result;
        }

        public async Task<Dictionary<string, List<AmazonProductResponseModelv2>>> GetAmazonProductDetailsFromListOfQueries(List<string> queries, int? nullableMaxPrice, int numOfProducts = 5)
        {
            int maxPrice = nullableMaxPrice ?? 50;
            var queryToProduct = new Dictionary<string, List<AmazonProductResponseModelv2>>();

            var tasks = queries.Select(async query =>
            {
                try
                {
                    var products = await this.ChooseAmazonProducts(query, maxPrice, numOfProducts);
                    queryToProduct.Add(query, products);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception calling the Amazon API: Exception={ex.Message} InnerException={ex.InnerException}");
                }
            });

            await Task.WhenAll(tasks);

            return queryToProduct;
        }

        public async Task<List<AmazonProductResponseModelv2>> ChooseAmazonProducts(string productQuery, int maxPrice, int numOfProducts)
        {
            List<AmazonProductResponseModelv2> productsToReturn = new List<AmazonProductResponseModelv2>();

            // search amazon for the query
            List<AmazonProductResponseModelv2> productList;
            try
            {
                var amazonResponseString = await this.SendApiRequestV2(productQuery);
                productList = JsonSerializer.Deserialize<List<AmazonProductResponseModelv2>>(amazonResponseString);
            }
            catch (Exception)
            {
                return new List<AmazonProductResponseModelv2>();
            }

            // Filter by max price
            productList.ForEach(x =>
            {
                if (x.StringPrice == string.Empty)
                {
                    x.Price = 0;
                }
                else
                {
                    var totalPriceString = x.StringPrice.Split(' ')[0];
                    x.Price = double.Parse(totalPriceString.Trim('$'));
                }
            });

            productList = productList.Where(x => x.Price <= maxPrice).OrderByDescending(x => x.Price).ToList();

            int numOfProductsLeft = productList.Count;
            if (numOfProductsLeft <= numOfProducts)
            {
                return productList.Take(numOfProductsLeft).ToList();
            }
            else
            {
                return productList.Take(numOfProducts).ToList();
            }
        }

        public async Task<string> SendApiRequestV2(string query)
        {
            var queryParam = this.NormalizeQueryIntoQueryParams(query);

            var retryPolicy = Policy.Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://{this.amazonProductConfiguration.ApiUrl}/search?keywords={queryParam}&marketplace=US"),
                    Headers =
                    {
                        { "X-RapidAPI-Key", this.rapidApiKey },
                        { "X-RapidAPI-Host", this.amazonProductConfiguration.ApiUrl },
                    },
                };
                using (var response = await retryPolicy.ExecuteAsync(() => client.SendAsync(request)))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    return body;
                }
            }
        }

        public async Task<string> SendApiRequestV1(string query)
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
                    return body;
                }
            }
        }

        public string CreateAffiliateLink(string ASIN)
        {
            return $"http://www.amazon.com/dp/{ASIN}/ref=nosim?tag={this.amazonAffiliateTag}";
        }

        private string NormalizeQueryIntoQueryParams(string query)
        {
            return query.Trim().Replace(" ", "%20");
        }
    }
}