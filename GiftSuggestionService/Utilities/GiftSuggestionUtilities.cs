using System;

namespace GiftSuggestionService.Utilities
{
    public static class GiftSuggestionUtilities
    {
        public static string GenerateGiftSuggestionIdFromName(string giftName)
        {
            return giftName.ToLower().Replace(' ', '-');
        }

        public static int NormalizeAge(int age)
        {
            return (int)Math.Round(age / 10.0);
        }

        public static int PercentLower(this int price, double percentage)
        {
            return price - (int)Math.Round(price * percentage);
        }

        public static int PercentHigher(this int price, double percentage)
        {
            return price + (int)Math.Round(price * percentage);
        }
    }
}