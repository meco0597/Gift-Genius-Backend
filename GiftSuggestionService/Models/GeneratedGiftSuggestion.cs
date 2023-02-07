using System;
using System.Collections.Generic;
using GiftSuggestionService.Dtos;

namespace GiftSuggestionService.Models
{
    public class GeneratedGiftSuggestionSchema
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<string> Categories { get; set; }

        public string EstimatedPriceRange { get; set; }
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
            var associatedAges = new List<int>();
            var associatedOccasions = new List<string>();
            var associatedSexes = new List<string>();
            if (searchParams.AssociatedAge != null)
            {
                associatedAges.Add((int)searchParams.AssociatedAge);
            }
            if (searchParams.AssociatedOccasion != null)
            {
                associatedOccasions.Add(searchParams.AssociatedOccasion);
            }
            if (searchParams.AssociatedSex != null)
            {
                associatedSexes.Add(searchParams.AssociatedSex);
            }

            return new GiftSuggestion()
            {
                GiftName = this.Name,
                GiftDescription = this.Description,
                AssociatedInterests = this.Categories,
                AssociatedAgeRanges = associatedAges,
                AssociatedOccasions = associatedOccasions,
                AssociatedSex = associatedSexes,
                MinPrice = this.EstimatedMinPrice,
                MaxPrice = this.EstimatedMaxPrice,
            };
        }
    }
}