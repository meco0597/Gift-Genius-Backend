using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using GiftSuggestionService.Models;
using GiftSuggestionService.Data;
using GiftSuggestionService.Dtos;
using GiftSuggestionService.Configurations;
using Microsoft.Extensions.Options;
using RestClient.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GiftSuggestionService.Services
{
    public class GptManagementService
    {
        private readonly string GptApiKey;
        private readonly EnivronmentConfiguration enivronmentConfiguration;
        private readonly GptConfiguration gptConfiguration;

        public GptManagementService(
            KeyvaultAccessorService keyvaultAccessorService,
            IOptionsMonitor<EnivronmentConfiguration> environmentOptionsMonitor,
            IOptionsMonitor<GptConfiguration> gptOptionsMonitor)
        {
            this.enivronmentConfiguration = environmentOptionsMonitor.CurrentValue;
            this.gptConfiguration = gptOptionsMonitor.CurrentValue;
            this.GptApiKey = keyvaultAccessorService.GetSecretAsync(this.gptConfiguration.SecretName).Result;
        }

        public List<GeneratedGiftSuggestion> GetGiftSuggestions(GiftSuggestionSearchDto searchParams, int numSuggestions = 10)
        {
            return TestData.TestGeneratedGiftSuggestions;
        }

        public string GeneratePromptFromParams(GiftSuggestionSearchDto searchParams, int numSuggestions = 10)
        {
            return $"{numSuggestions.ToString()} {searchParams.AssociatedOccasion} gift ideas for a {searchParams.AssociatedAge}-year-old"
            + $" {searchParams.AssociatedSex} with an interest in {string.Join(", ", searchParams.AssociatedInterests)} between ${searchParams.MinPrice} and ${searchParams.MaxPrice}";
        }

        public async Task<string> CreateCompletionAsync(string prompt)
        {
            var requestBody = new CompletionsRequestModel()
            {
                Prompt = prompt,
                Model = "text-davinci-003",
                MaxTokens = 1024,
                Temperature = 0.2
            };

            using (var client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://api.openai.com/v1/completions");

                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody));
                client.BaseAddress = new Uri("https://api.openai.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.GptApiKey}");

                HttpResponseMessage response = await client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("success!");
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }

                return response.Content.ToString();
            }
        }
    }
}