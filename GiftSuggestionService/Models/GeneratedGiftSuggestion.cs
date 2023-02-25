using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using GiftSuggestionService.Dtos;

namespace GiftSuggestionService.Models
{
    public class GeneratedGiftSuggestionSchema
    {
        [JsonPropertyName("giftIdea")]
        public string Name { get; set; }

        [JsonPropertyName("giftDescription")]
        public string Description { get; set; }

        [JsonPropertyName("giftCategories")]
        public List<string> Categories { get; set; }

        [JsonPropertyName("estimatedPrice")]
        public string EstimatedPriceRange { get; set; }

        public GeneratedGiftSuggestion ToGeneratedModel(GeneratedGiftSuggestionSchema unParsed)
        {
            string[] minAndMax = unParsed.EstimatedPriceRange.Split("-");
            return new GeneratedGiftSuggestion()
            {
                Name = unParsed.Name,
                Description = unParsed.Description,
                Categories = unParsed.Categories,
                EstimatedMinPrice = Int32.Parse(minAndMax[0]),
                EstimatedMaxPrice = Int32.Parse(minAndMax[1]),
            };
        }
    }

    public class GeneratedGiftSuggestion
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<string> Categories { get; set; }

        public int EstimatedMaxPrice { get; set; }

        public int EstimatedMinPrice { get; set; }

        public GiftSuggestion ToPrivateModel(GiftSuggestionSearchDto searchParams)
        {
            var associatedAges = new List<AgeDescriptor>();
            var associatedRelationships = new List<RelationshipDescriptor>();
            if (searchParams.AssociatedAge != null)
            {
                associatedAges.Add((AgeDescriptor)searchParams.AssociatedAge);
            }
            if (searchParams.AssociatedRelationship != null)
            {
                associatedRelationships.Add((RelationshipDescriptor)searchParams.AssociatedRelationship);
            }

            return new GiftSuggestion()
            {
                GiftName = this.Name,
                GiftDescription = this.Description,
                AssociatedInterests = this.Categories,
                AssociatedAgeRanges = associatedAges,
                AssociatedRelationships = associatedRelationships,
                MinPrice = this.EstimatedMinPrice,
                MaxPrice = this.EstimatedMaxPrice,
            };
        }
    }
}