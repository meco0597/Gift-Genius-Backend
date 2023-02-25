using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using GiftSuggestionService.Models;
using GiftSuggestionService.Data;
using System.Text.Json;
using GiftSuggestionService.Dtos;
using GiftSuggestionService.Configurations;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Models;

namespace GiftSuggestionService.Services
{
    public class GptManagementService
    {
        private readonly string GptApiKey;
        private readonly EnivronmentConfiguration enivronmentConfiguration;
        private readonly GptConfiguration gptConfiguration;
        private readonly OpenAIAPI api;

        public GptManagementService(
            KeyvaultAccessorService keyvaultAccessorService,
            IOptionsMonitor<EnivronmentConfiguration> environmentOptionsMonitor,
            IOptionsMonitor<GptConfiguration> gptOptionsMonitor)
        {
            this.enivronmentConfiguration = environmentOptionsMonitor.CurrentValue;
            this.gptConfiguration = gptOptionsMonitor.CurrentValue;
            this.GptApiKey = keyvaultAccessorService.GetSecretAsync(this.gptConfiguration.SecretName).Result;
            this.api = new OpenAIAPI(new APIAuthentication(this.GptApiKey));
        }

        public async Task<List<GeneratedGiftSuggestion>> GetGiftSuggestions(GiftSuggestionSearchDto searchParams)
        {
            string prompt = this.GeneratePromptFromParamsV1(searchParams);
            string response = await this.CreateCompletionAsync(prompt);
            Console.WriteLine(response);
            List<GeneratedGiftSuggestionSchema> generatedGiftSuggestions = System.Text.Json.JsonSerializer.Deserialize<List<GeneratedGiftSuggestionSchema>>(response);

            List<GeneratedGiftSuggestion> toReturn = new List<GeneratedGiftSuggestion>();
            generatedGiftSuggestions.ForEach(x =>
            {
                string[] minAndMax = x.EstimatedPriceRange.Split("-");
                toReturn.Add(new GeneratedGiftSuggestion()
                {
                    Name = x.Name.Trim(),
                    Description = x.Description.Trim(),
                    Categories = x.Categories,
                    EstimatedMinPrice = Int32.Parse(minAndMax[0]),
                    EstimatedMaxPrice = Int32.Parse(minAndMax[1]),
                });
            });

            return toReturn;
        }

        public string GeneratePromptFromParamsV1(GiftSuggestionSearchDto searchParams)
        {
            return "Gift recipient: 5 gifts for my Mom in her Fifties that has an interest in cooking and singing, between $25 and $100 in valid JSON\nGift suggestions:\n[ { \"giftIdea\": \"Cookbook by a celebrity chef\", \"giftCategories\": [ \"cooking\" ], \"estimatedPrice\": \"30-45\", \"giftDescription\": \"A collection of recipes, instructions, and information about the preparation and serving of foods. This gift will allow them to explore unique cooking recipes.\" }, { \"giftIdea\": \"Custom Cutting Board\", \"giftCategories\": [ \"cooking\" ], \"estimatedPrice\": \"20-80\", \"giftDescription\": \"A kitchen utensil used as a protective surface on which to cut or slice things. Add a personal touch by customizing it with their name or initials.\" }, { \"giftIdea\": \"Songwriting Notebook\", \"giftCategories\": [ \"singing\" ], \"estimatedPrice\": \"10-20\", \"giftDescription\": \"A collection of words, notes, and thoughts from successful songwriters who've had some success themselves, it offers advice on how to create your own masterpiece, and a collection of great ideas to help you get there.\" }, { \"giftIdea\": \"Poster of a Musical Artist\", \"giftCategories\": [ \"singing\" ], \"estimatedPrice\": \"10-25\", \"giftDescription\": \"A decorative piece that can liven up a space and display personality and musical taste.\" }, { \"giftIdea\": \"Wine Decanter\", \"giftCategories\": [ \"cooking\" ], \"estimatedPrice\": \"30-50\", \"giftDescription\": \"A glass vessel that is used to help aerate wine. For the home cook who serves wine with every meal.\" } ]\n\n" +
            $"Gift recipient: 5 gifts for my {searchParams.AssociatedRelationship} in {searchParams.Pronoun} {searchParams.AssociatedAge} that has an interest in {string.Join(", ", searchParams.AssociatedInterests)} between ${searchParams.MinPrice} and ${searchParams.MaxPrice} in valid JSON\nGift suggestions:";
        }

        public string GeneratePromptFromParamsCompact(GiftSuggestionSearchDto searchParams)
        {
            return "Gift recipient: 5 gifts for my Mom in her Fifties that has an interest in cooking and singing, between $25 and $100\nGift suggestions:\n\nGift Idea: Cookbook by a celebrity chef\nGift Categories: cooking\n\nGift Idea: Custom Cutting Board\nGift Categories: cooking\n\nGift Idea: Songwriting Notebook\nGift Categories: singing\n\nGift Idea: Poster of a Musical Artist\nGift Categories: singing\n\nGift Idea: Wine Decanter\nGift Categories: cooking\n\n" +
            $"Gift recipient: 5 gifts for my {searchParams.AssociatedRelationship} in {searchParams.Pronoun} {searchParams.AssociatedAge} that has an interest in {string.Join(", ", searchParams.AssociatedInterests)} between ${searchParams.MinPrice} and ${searchParams.MaxPrice}\nGift suggestions:";
        }

        public async Task<string> CreateCompletionAsync(string prompt)
        {
            var result = await api.Completions.CreateCompletionAsync(new CompletionRequest(
                prompt,
                model: Model.DavinciText,
                temperature: 0.1,
                max_tokens: 2048,
                top_p: 1,
                frequencyPenalty: 0.5,
                presencePenalty: 0));

            return result.Completions.First().Text;
        }
    }
}