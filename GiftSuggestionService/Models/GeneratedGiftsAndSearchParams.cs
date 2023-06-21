using System.Collections.Generic;
using GiftSuggestionService.Dtos;

namespace GiftSuggestionService.Models
{
    public class GeneratedGiftsAndSearchParams
    {
        public List<GeneratedGiftSuggestion> GeneratedGiftSuggestions { get; set; }

        public GiftSuggestionSearchDto SearchParams { get; set; }
    }
}