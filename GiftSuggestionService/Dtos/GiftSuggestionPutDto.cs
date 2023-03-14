namespace GiftSuggestionService.Dtos
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using GiftSuggestionService.Models;

    public class GiftSuggestionPutDto
    {
        public string GiftName { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public List<string> AssociatedInterests { get; set; }

        public List<RelationshipDescriptor> AssociatedRelationships { get; set; }

        public List<AgeDescriptor> AssociatedAgeRanges { get; set; }

        public long NumOfUpvotes { get; set; }
    }
}