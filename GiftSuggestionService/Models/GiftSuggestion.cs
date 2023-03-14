using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GiftSuggestionService.Models
{
    public class GiftSuggestion
    {
        public string Id { get; set; }

        public string GiftName { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<string> ProductIds { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public List<string> AssociatedInterests { get; set; }

        public List<RelationshipDescriptor> AssociatedRelationships { get; set; }

        public List<AgeDescriptor> AssociatedAgeRanges { get; set; }

        public long NumOfUpvotes { get; set; }

        public long NumOfClicks { get; set; }

        public long NumOfTimesSuggested { get; set; }
    }
}