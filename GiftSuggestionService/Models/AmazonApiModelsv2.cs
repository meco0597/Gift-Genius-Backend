namespace GiftSuggestionService.Models
{
    using System.Text.Json.Serialization;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public class AmazonProductResponseModelv2
    {
        [JsonPropertyName("ASIN")]
        public string ASIN { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("price")]
        public string StringPrice { get; set; }

        [JsonPropertyName("listPrice")]
        public string ListPrice { get; set; }

        [JsonPropertyName("imageUrl")]
        public string ThumbnailUrl { get; set; }

        [JsonPropertyName("detailPageURL")]
        public string Url { get; set; }

        [JsonPropertyName("rating")]
        public string Rating { get; set; }

        [JsonPropertyName("totalReviews")]
        public string TotalReviews { get; set; }

        [JsonPropertyName("subtitle")]
        public string Subtitle { get; set; }

        [JsonPropertyName("isPrimeEligible")]
        public string AmazonPrime { get; set; }

        [JsonIgnore]
        public double Price { get; set; }
    }
}