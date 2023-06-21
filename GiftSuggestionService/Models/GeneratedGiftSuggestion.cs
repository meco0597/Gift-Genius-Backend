using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using GiftSuggestionService.Dtos;

namespace GiftSuggestionService.Models
{
    public class GeneratedGiftSuggestion
    {
        public string Name { get; set; }

        public List<string> Categories { get; set; }

        public GiftSuggestion ToPrivateModel(GiftSuggestionSearchDto searchParams)
        {
            var associatedAges = new List<AgeDescriptor>();
            var associatedRelationships = new List<RelationshipDescriptor>();
            var associatedPronouns = new List<Pronoun>();
            int minPrice = int.MaxValue;
            int maxPrice = int.MinValue;
            associatedRelationships.Add((RelationshipDescriptor)searchParams.AssociatedRelationship);
            associatedAges.Add((AgeDescriptor)searchParams.AssociatedAge);
            if (searchParams.MinPrice != null)
            {
                minPrice = (int)searchParams.MinPrice;
            }
            if (searchParams.MaxPrice != null)
            {
                maxPrice = (int)searchParams.MaxPrice;
            }

            return new GiftSuggestion()
            {
                GiftName = this.Name,
                AssociatedInterests = this.Categories,
                AssociatedAgeRanges = associatedAges,
                AssociatedRelationships = associatedRelationships,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                AssociatedPronouns = associatedPronouns,
            };
        }
    }
}