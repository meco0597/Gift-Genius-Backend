
using System;

namespace GiftSuggestionService.Models
{
    public class Product
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string ThumbnailUrl { get; set; }

        public double Price { get; set; }

        public long NumOfUpvotes { get; set; }

        public long NumOfClicks { get; set; }

        public long NumOfTimesSuggested { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }
}