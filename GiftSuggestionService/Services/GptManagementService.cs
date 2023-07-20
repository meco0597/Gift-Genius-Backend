using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using GiftSuggestionService.Models;
using GiftSuggestionService.Dtos;
using GiftSuggestionService.Configurations;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using OpenAI_API.Chat;
using Polly;
using Polly.Retry;
using System.Net.Http;

namespace GiftSuggestionService.Services
{
    public class GptManagementService
    {
        private readonly string GptApiKey;
        private readonly EnivronmentConfiguration enivronmentConfiguration;
        private readonly RequestAccessorService requestAccessorService;
        private readonly GptConfiguration gptConfiguration;
        private readonly OpenAIAPI api;
        private readonly Dictionary<Pronoun, string> pronounMapping = new Dictionary<Pronoun, string>()
        {
            { Pronoun.He, "his" },
            { Pronoun.She, "her" },
            { Pronoun.They, "their" },
        };
        private readonly Dictionary<Pronoun, string> pronounPossesive = new Dictionary<Pronoun, string>()
        {
            { Pronoun.He, "is" },
            { Pronoun.She, "is" },
            { Pronoun.They, "are" },
        };
        private AsyncRetryPolicy retryPolicy = Policy
            .Handle<HttpRequestException>() // Adjust the exception type based on your needs
            .WaitAndRetryAsync(
                retryCount: 3, // Number of retries
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Exponential backoff
                onRetry: (exception, calculatedWaitDuration) =>
                {
                    // Optionally log or handle the retry attempt here
                    Console.WriteLine($"Request to OpenAI failed. Retry due to exception: {exception.Message}. Retrying in {calculatedWaitDuration.TotalSeconds} seconds...");
                });


        public GptManagementService(
            KeyvaultAccessorService keyvaultAccessorService,
            RequestAccessorService requestAccessorService,
            IOptionsMonitor<EnivronmentConfiguration> environmentOptionsMonitor,
            IOptionsMonitor<GptConfiguration> gptOptionsMonitor)
        {
            this.enivronmentConfiguration = environmentOptionsMonitor.CurrentValue;
            this.gptConfiguration = gptOptionsMonitor.CurrentValue;
            this.requestAccessorService = requestAccessorService;
            this.GptApiKey = keyvaultAccessorService.GetSecretAsync(this.gptConfiguration.SecretName).Result;
            this.api = new OpenAIAPI(new APIAuthentication(this.GptApiKey));
        }

        public async Task<List<GeneratedGiftSuggestion>> GetProductGiftSuggestions(GiftSuggestionSearchDto searchParams)
        {
            List<ChatMessage> chatProductPrompt = this.GenerateChatMessagesForProductSuggestions(searchParams);
            string productResponse = await retryPolicy.ExecuteAsync(async () =>
            {
                return await this.SendChatRequestToOpenAi(chatProductPrompt);
            });

            var generatedGiftSuggestions = this.ParseGptResponse(productResponse);

            return generatedGiftSuggestions;
        }

        public async Task<List<GeneratedGiftSuggestion>> GetGenericGiftSuggestions(GiftSuggestionSearchDto searchParams)
        {
            List<ChatMessage> chatGenericPrompt = this.GenerateChatMessagesForGenericSuggestions(searchParams);
            string response = await retryPolicy.ExecuteAsync(async () =>
            {
                return await this.SendChatRequestToOpenAi(chatGenericPrompt);
            });

            var generatedGiftSuggestions = this.ParseGptResponse(response);

            return generatedGiftSuggestions;
        }

        public List<ChatMessage> GenerateChatMessagesForProductSuggestions(GiftSuggestionSearchDto searchParams)
        {
            return new List<ChatMessage>()
            {
                new ChatMessage(ChatMessageRole.System, "You are a gifting expert. Given a budget, occasion, age, sex, and interests, suggest up to 5 gift ideas within 20% of the max price. Use brand names when appropriate. In case the age or relationship don't make sense, don't ask for any corrections or clarifications, only respond with search queries based on the interests."),
                new ChatMessage(ChatMessageRole.User, "Come up with gift suggestions for my mom. She is in her fifties, and she has an interest in cooking and sewing. Max price is $100. Occasion is Birthday."),
                new ChatMessage(ChatMessageRole.Assistant, "Gift Idea: Instant Pot Duo 7-in-1 Electric Pressure Cooker\nGift Categories: cooking\n\nGift Idea: Homemade Pasta Making Set\nGift Categories: cooking\n\nGift Idea: Embroidery Kit\nGift Categories: sewing\n\nGift Idea: Customized Apron\nGift Categories: cooking\n\nGift Idea: Cute Sewing Pin Cushion\nGift Categories: sewing"),
                new ChatMessage(ChatMessageRole.User, $"{this.GetProductPromptVerbage(searchParams.AssociatedRelationship, searchParams.AssociatedAge, searchParams.Pronoun, searchParams.AssociatedInterests, searchParams.Occasion, (int)searchParams.MaxPrice)}")
            };
        }

        public List<ChatMessage> GenerateChatMessagesForGenericSuggestions(GiftSuggestionSearchDto searchParams)
        {
            return new List<ChatMessage>()
            {
                new ChatMessage(ChatMessageRole.System, "You are a gifting expert. Given a budget, occasion, age, sex, and interests, suggest up to 4 Amazon search queries for gifts. In case the age or relationship don't make sense, don't ask for any corrections or clarifications, only respond with search queries based on the interests."),
                new ChatMessage(ChatMessageRole.User, "Come up with amazon gift queries for my mom. She is in her fifties, and she has an interest in cooking and sewing. Max price is $100. Occasion is Birthday."),
                new ChatMessage(ChatMessageRole.Assistant, "Gift Idea: Cooking birthday gifts for mom\nGift Categories: cooking\n\nGift Idea: Sewing gifts for mom\nGift Categories: sewing\n\nGift Idea: Birthday gifts for Mom over $50\nGift Categories: Mom\n\nGift Idea: Funny cooking Gifts for Mom\nGift Categories: cooking"),
                new ChatMessage(ChatMessageRole.User, $"{this.GetProductPromptVerbage(searchParams.AssociatedRelationship, searchParams.AssociatedAge, searchParams.Pronoun, searchParams.AssociatedInterests, searchParams.Occasion, (int)searchParams.MaxPrice, false)}")
            };
        }

        public string GenerateCompletionPromptFromParamsV1(GiftSuggestionSearchDto searchParams)
        {
            return "Gift recipient: My Mom in her Fifties that has an interest in cooking and singing\n5 Gift suggestions under $100:\n\nGift Idea: Cookbook by a celebrity chef\nGift Categories: cooking\n\nGift Idea: Custom Cutting Board\nGift Categories: cooking\n\nGift Idea: Songwriting Notebook\nGift Categories: singing\n\nGift Idea: Poster of a Musical Artist\nGift Categories: singing\n\nGift Idea: Wine Decanter\nGift Categories: cooking\n\n" +
            $"Gift recipient: My {searchParams.AssociatedRelationship} in {searchParams.Pronoun} {searchParams.AssociatedAge} that has an interest in {string.Join(", ", searchParams.AssociatedInterests)}\n5 Gift suggestions under ${searchParams.MaxPrice}:";
        }

        public async Task<string> SendChatRequestToOpenAi(List<ChatMessage> messages)
        {
            var result = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.ChatGPTTurbo,
                Messages = messages,
                Temperature = 0.7,
                MaxTokens = 1024,
                TopP = 1,
                FrequencyPenalty = 0.5,
                PresencePenalty = 0.1
            });

            string gptResponse = result.Choices.First().Message.Content;
            Console.WriteLine($"Response from OpenAi: Prompt={messages[3].Content} Response={this.TranslateGptResponseForLog(gptResponse)} Latency={result.ProcessingTime} Tokens={result.Usage.TotalTokens}");
            return gptResponse;
        }

        private List<GeneratedGiftSuggestion> ParseGptResponse(string gptResponse)
        {
            List<GeneratedGiftSuggestion> toReturn = new List<GeneratedGiftSuggestion>();
            List<string> ideasAndCategories = new List<string>();

            // Use regular expression to match the gift idea and category
            Regex regex = new Regex(@"Gift Idea\s*:\s*(.+)\nGift Categories\s*:\s*(.+)");
            MatchCollection matches = regex.Matches(gptResponse);

            // Loop through the matches and add them to the dictionary
            foreach (Match match in matches)
            {
                string giftIdea = match.Groups[1].Value;
                List<string> categories = match.Groups[2].Value.Split(',', options: StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

                Console.WriteLine($"Generated Gift Idea: CorrelationId={this.requestAccessorService.GetCorrelationId()} Name={giftIdea} Category={string.Join(", ", categories)}");
                toReturn.Add(new GeneratedGiftSuggestion()
                {
                    Name = giftIdea,
                    Categories = categories,
                });
            }

            return toReturn;
        }

        private string GetProductPromptVerbage(RelationshipDescriptor relationship, AgeDescriptor ageDescriptor, Pronoun pronoun, List<string> associatedInterests, string occasion, int maxPrice, bool isForProduct = true)
        {
            // Come up with gift suggestions for my mom. She is in her Fifties and she has an interest in cooking, and sewing. Max price is $50.
            string toReturn = $"Come up with {(isForProduct ? "gift suggestions" : "amazon gift queries")} for my {relationship}. {pronoun} {this.pronounPossesive[pronoun] ?? "is"}";

            switch (ageDescriptor)
            {
                case AgeDescriptor.Toddler:
                    toReturn += $" a {ageDescriptor}";
                    break;
                case AgeDescriptor.Preschool:
                case AgeDescriptor.Gradeschool:
                case AgeDescriptor.MiddleSchool:
                    toReturn += $" in {ageDescriptor}";
                    break;
                default:
                    toReturn += $" in {this.pronounMapping[pronoun]} {ageDescriptor}";
                    break;
            }

            toReturn += $", and {pronoun} {(pronoun == Pronoun.They ? "have" : "has")} an interest in {this.JoinListOfInterests(associatedInterests)}. Max price is ${maxPrice}. Occasion is {occasion}";
            return toReturn;
        }

        private string JoinListOfInterests(List<string> interests)
        {
            if (interests.Count == 1)
            {
                return interests[0];
            }
            else if (interests.Count == 2)
            {
                return string.Join(" and ", interests);
            }
            else
            {
                string allButLast = string.Join(", ", interests.GetRange(0, interests.Count - 1));
                return $"{allButLast}, and {interests[interests.Count - 1]}";
            }
        }

        private string GetAgeVerbage(AgeDescriptor ageDescriptor)
        {
            switch (ageDescriptor)
            {
                case AgeDescriptor.Toddler:
                case AgeDescriptor.Preschool:
                case AgeDescriptor.Gradeschool:
                case AgeDescriptor.MiddleSchool:
                    return $"{ageDescriptor} years";
                default:
                    return ageDescriptor.ToString();
            }
        }

        private string GetGiftCategoriesFromLine(string line)
        {
            int giftCategoriesIndex = line.IndexOf(":");
            return line.Substring(giftCategoriesIndex + 1, line.Length - (giftCategoriesIndex + 1)).Trim();
        }

        private string TranslateGptResponseForLog(string response)
        {
            return response.Replace("\n\n", "-------").Replace('\n', '|');
        }
    }
}