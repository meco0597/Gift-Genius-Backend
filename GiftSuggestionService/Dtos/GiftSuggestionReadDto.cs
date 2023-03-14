namespace GiftSuggestionService.Dtos
{
    using System;
    using System.Collections.Generic;
    using GiftSuggestionService.Models;

    public class GiftSuggestionReadDto
    {
        public string Id { get; set; }

        public string GiftName { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Link { get; set; }

        public string ThumbnailUrl { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public List<string> AssociatedInterests { get; set; }

        public List<RelationshipDescriptor> AssociatedRelationships { get; set; }

        public List<AgeDescriptor> AssociatedAgeRanges { get; set; }

        public long NumOfUpvotes { get; set; }
    }
}