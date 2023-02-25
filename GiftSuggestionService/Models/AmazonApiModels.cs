namespace GiftSuggestionService.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class AmazonProductSearchResponseModel
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("result")]
        public List<AmazonProductResponseModel> Result { get; set; }

        [JsonPropertyName("totalProducts")]
        public string TotalProducts { get; set; }
    }

    public class AmazonProductResponseModel
    {
        [JsonPropertyName("position")]
        public AmazonPositionModel Position { get; set; }

        [JsonPropertyName("asin")]
        public string ASIN { get; set; }

        [JsonPropertyName("price")]
        public AmazonPriceModel Price { get; set; }

        [JsonPropertyName("reviews")]
        public AmazonReviewsModel Reviews { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("score")]
        public string Score { get; set; }

        [JsonPropertyName("sponsored")]
        public bool Sponsored { get; set; }

        [JsonPropertyName("amazonChoice")]
        public bool AmazonChoice { get; set; }

        [JsonPropertyName("bestSeller")]
        public bool BestSeller { get; set; }

        [JsonPropertyName("amazonPrime")]
        public bool AmazonPrime { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("thumbnail")]
        public string ThumbnailUrl { get; set; }
    }

    public class AmazonPositionModel
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("global_position")]
        public int GlobalPosition { get; set; }
    }

    public class AmazonPriceModel
    {
        [JsonPropertyName("discounted")]
        public bool Discounted { get; set; }

        [JsonPropertyName("current_price")]
        public double CurrentPrice { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("before_price")]
        public double BeforePrice { get; set; }

        [JsonPropertyName("savings_amount")]
        public double SavingsAmount { get; set; }

        [JsonPropertyName("savings_percent")]
        public double SavingsPercent { get; set; }
    }

    public class AmazonReviewsModel
    {
        [JsonPropertyName("total_reviews")]
        public int TotalReviews { get; set; }

        [JsonPropertyName("rating")]
        public double Rating { get; set; }
    }
}