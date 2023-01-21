using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GiftSuggestionService.Models
{
    public class GiftSuggestion
    {
        [BsonId]
        public Guid Id { get; set; }

        public string GiftName { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Uri Link { get; set; } = null!;

        public int MinPrice { get; set; } = 0;

        public int MaxPrice { get; set; } = int.MaxValue;

        public GiftKind GiftKind { get; set; } = GiftKind.RetailItem;

        public string GiftDescription { get; set; } = null!;

        public List<string> AssociatedInterests { get; set; } = null!;

        public List<string> AssociatedOccasions { get; set; } = null!;

        public List<string> AssociatedSex { get; set; } = null!;

        public List<int> AssociatedAgeRanges { get; set; } = null!;

        public long NumOfUpvotes { get; set; } = 0;
    }
}