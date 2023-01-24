using System;

namespace GiftSuggestionService.Utilities
{
    public static class GiftSuggestionUtilities
    {
        public static string GenerateGiftSuggestionIdFromName(string giftName)
        {
            return giftName.ToLower().Replace(' ', '-');
        }
    }
}