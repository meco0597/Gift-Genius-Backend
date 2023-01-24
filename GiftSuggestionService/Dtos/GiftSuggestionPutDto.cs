namespace GiftSuggestionService.Dtos
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using GiftSuggestionService.Models;

    public class GiftSuggestionPutDto
    {
        public string GiftName { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public string GiftDescription { get; set; }

        public List<string> AssociatedInterests { get; set; }

        public List<string> AssociatedOccasions { get; set; }

        public List<string> AssociatedSex { get; set; }

        public List<int> AssociatedAgeRanges { get; set; }

        public long NumOfUpvotes { get; set; }
    }
}