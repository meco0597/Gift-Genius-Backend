using System;
using System.Collections.Generic;
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

namespace GiftSuggestionService.Services
{
    public class GptManagementService
    {
        private readonly string GptApiKey;
        private readonly EnivronmentConfiguration enivronmentConfiguration;
        private readonly RequestAccessorService requestAccessorService;
        private readonly GptConfiguration gptConfiguration;
        private readonly OpenAIAPI api;

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

        public async Task<List<GeneratedGiftSuggestion>> GetGiftSuggestions(GiftSuggestionSearchDto searchParams, string version = "ChatV1")
        {
            if (version == "CompletionsV1")
            {
                string prompt = this.GenerateCompletionPromptFromParamsV1(searchParams);
                string response = await this.CreateCompletionAsync(prompt);
                var generatedGiftSuggestions = this.ParseGptResponse(response);
                return generatedGiftSuggestions;
            }
            else if (version == "ChatV1")
            {
                List<ChatMessage> chatPrompt = this.GenerateChatMessagesV1(searchParams);
                string response = await this.CreateChatRequestAsync(chatPrompt);
                var generatedGiftSuggestions = this.ParseGptResponse(response);
                return generatedGiftSuggestions;
            }
            else
            {
                throw new ArgumentException($"GPT prompt version unknown: {version}");
            }
        }

        public List<ChatMessage> GenerateChatMessagesV1(GiftSuggestionSearchDto searchParams)
        {
            return new List<ChatMessage>()
            {
                new ChatMessage(ChatMessageRole.System, "You are a gifting expert who helps people find creative, useful, and unique gift ideas. You will be given a max price and information about the gift recipients age, sex, and interests, as well as the relationship to the person seeking the gift ideas. You will then provide 10 gift suggestions that match these constraints that are can be found on Amazon."),
                new ChatMessage(ChatMessageRole.User, "Come up with gift suggestions for my mom in her Fifties that has an interest in cooking, singing that are under $100"),
                new ChatMessage(ChatMessageRole.Assistant, "Gift Idea: Instant Pot Duo 7-in-1 Electric Pressure Cooker\nGift Categories: cooking\n\nGift Idea: Personalized Recipe Book\nGift Categories: cooking\n\nGift Idea: Singing Machine Karaoke System\nGift Categories: singing\n\nGift Idea: Music-themed Kitchen Accessories\nGift Categories: singing, cooking\n\nGift Idea: Cookware Set\nGift Categories: cooking\n\nGift Idea: Cookbook Stand\nGift Categories: cooking\n\nGift Idea: Customized Apron\nGift Categories: cooking\n\nGift Idea: Online Cooking Class Subscription\nGift Categories: cooking\n\nGift Idea: Wireless Bluetooth Microphone\nGift Categories: singing\n\nGift Idea: Herb Garden Starter Kit\nGift Categories: cooking"),
                new ChatMessage(ChatMessageRole.User, $"Come up with gift suggestions for my {searchParams.AssociatedRelationship} in {searchParams.Pronoun} {this.GetAgeVerbage(searchParams.AssociatedAge)}, that has an interest in {string.Join(", ", searchParams.AssociatedInterests)} that are under ${searchParams.MaxPrice}")
            };
        }

        public string GenerateCompletionPromptFromParamsV1(GiftSuggestionSearchDto searchParams)
        {
            return "Gift recipient: My Mom in her Fifties that has an interest in cooking and singing\n5 Gift suggestions under $100:\n\nGift Idea: Cookbook by a celebrity chef\nGift Categories: cooking\n\nGift Idea: Custom Cutting Board\nGift Categories: cooking\n\nGift Idea: Songwriting Notebook\nGift Categories: singing\n\nGift Idea: Poster of a Musical Artist\nGift Categories: singing\n\nGift Idea: Wine Decanter\nGift Categories: cooking\n\n" +
            $"Gift recipient: My {searchParams.AssociatedRelationship} in {searchParams.Pronoun} {searchParams.AssociatedAge} that has an interest in {string.Join(", ", searchParams.AssociatedInterests)}\n5 Gift suggestions under ${searchParams.MaxPrice}:";
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

            Console.WriteLine($"Response from OpenAi: Prompt Version=CompletionsV1 Latency={result.ProcessingTime} Tokens={result.Usage.TotalTokens} Prompt={prompt}");
            return result.Completions.First().Text;
        }

        public async Task<string> CreateChatRequestAsync(List<ChatMessage> messages)
        {
            var result = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.ChatGPTTurbo,
                Messages = messages,
                Temperature = 0.7,
                MaxTokens = 2048,
                TopP = 1,
                FrequencyPenalty = 0.5,
                PresencePenalty = 0.1
            });

            Console.WriteLine($"Response from OpenAi: Prompt Version=ChatV1 Latency={result.ProcessingTime} Tokens={result.Usage.TotalTokens} Request Message={messages[3].Content}");
            return result.Choices.First().Message.Content;
        }

        private List<GeneratedGiftSuggestion> ParseGptResponse(string gptResponse)
        {
            List<GeneratedGiftSuggestion> toReturn = new List<GeneratedGiftSuggestion>();
            List<string> ideasAndCategories = new List<string>();

            var lines = gptResponse.Split('\n', options: StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                var tokens = line.Split(' ', options: StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

                int giftIdeaIndex = tokens.IndexOf("Idea:");
                int giftCategoriesIndex = tokens.IndexOf("Categories:");

                if (giftIdeaIndex > -1)
                {
                    string giftName = string.Join(' ', tokens.GetRange(giftIdeaIndex + 1, tokens.Count - (giftIdeaIndex + 1)));
                    ideasAndCategories.Add(giftName);
                }

                if (giftCategoriesIndex > -1)
                {
                    string giftCategories = this.GetGiftCategoriesFromLine(line);
                    ideasAndCategories.Add(giftCategories);
                }
            }

            for (int i = 0; i < ideasAndCategories.Count; i += 2)
            {
                var categories = ideasAndCategories[i + 1].Split(',', options: StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();
                Console.WriteLine($"Generated Gift Idea: CorrelationId={this.requestAccessorService.GetCorrelationId()} Name={ideasAndCategories[i]} Category={string.Join(", ", categories)}");
                toReturn.Add(new GeneratedGiftSuggestion()
                {
                    Name = ideasAndCategories[i],
                    Categories = categories,
                });
            }

            return toReturn;
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
    }
}