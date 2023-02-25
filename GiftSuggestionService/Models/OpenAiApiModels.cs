namespace GiftSuggestionService.Models
{
    using Newtonsoft.Json;

    public class CompletionsRequestModel
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("prompt")]
        public string Prompt { get; set; }

        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonProperty("temperature")]
        public double Temperature { get; set; }

        [JsonProperty("top_p")]
        public double TopP { get; set; }

        [JsonProperty("frequency_penalty")]
        public double FrequencyPenalty { get; set; }

        [JsonProperty("presence_penalty")]
        public double PresencePenalty { get; set; }

        [JsonProperty("n")]
        public int N { get; set; }

        [JsonProperty("stream")]
        public bool Stream { get; set; }

        [JsonProperty("stop")]
        public string Stop { get; set; }
    }

    public class CompletionsResponseModel
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("prompt")]
        public string Prompt { get; set; }

        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonProperty("temperature")]
        public double Temperature { get; set; }

        [JsonProperty("top_p")]
        public double TopP { get; set; }

        [JsonProperty("n")]
        public int N { get; set; }

        [JsonProperty("stream")]
        public bool Stream { get; set; }

        [JsonProperty("stop")]
        public string Stop { get; set; }
    }
}