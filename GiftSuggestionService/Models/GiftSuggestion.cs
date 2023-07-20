using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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

        [BsonRepresentation(BsonType.String)]
        public List<RelationshipDescriptor> AssociatedRelationships { get; set; }

        [BsonRepresentation(BsonType.String)]
        public List<AgeDescriptor> AssociatedAgeRanges { get; set; }

        [BsonRepresentation(BsonType.String)]
        public List<Pronoun> AssociatedPronouns { get; set; }

        public List<string> AssociatedOccasions { get; set; }

        public long NumOfUpvotes { get; set; }

        public long NumOfClicks { get; set; }

        public long NumOfTimesSuggested { get; set; }
    }
}