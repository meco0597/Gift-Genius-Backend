namespace GiftSuggestionService.Dtos
{
    using System.Collections.Generic;

    public class GiftSuggestionSearchDto
    {
        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }

        public List<string> AssociatedInterests { get; set; }

        public string AssociatedOccasion { get; set; }

        public string AssociatedSex { get; set; }

        public int? AssociatedAge { get; set; }
    }
}