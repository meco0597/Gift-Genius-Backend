using System;
using System.Collections.Generic;
using GiftSuggestionService.Models;

namespace GiftSuggestionService.Utilities
{
    public static class GiftSuggestionUtilities
    {
        public static string GenerateGiftSuggestionIdFromName(string giftName)
        {
            return giftName.Trim().ToLower().Replace(' ', '-');
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

        public static List<AgeDescriptor> GetNearbyAges(this AgeDescriptor ageDescriptor)
        {
            List<AgeDescriptor> toReturn = new List<AgeDescriptor>();
            toReturn.Add(ageDescriptor);

            switch (ageDescriptor)
            {
                case AgeDescriptor.Nineties:
                    toReturn.Add(AgeDescriptor.Sixties);
                    toReturn.Add(AgeDescriptor.Seventies);
                    toReturn.Add(AgeDescriptor.Eighties);
                    break;
                case AgeDescriptor.Eighties:
                    toReturn.Add(AgeDescriptor.Sixties);
                    toReturn.Add(AgeDescriptor.Seventies);
                    toReturn.Add(AgeDescriptor.Nineties);
                    break;
                case AgeDescriptor.Seventies:
                    toReturn.Add(AgeDescriptor.Sixties);
                    toReturn.Add(AgeDescriptor.Eighties);
                    toReturn.Add(AgeDescriptor.Nineties);
                    break;
                case AgeDescriptor.Sixties:
                    toReturn.Add(AgeDescriptor.Seventies);
                    toReturn.Add(AgeDescriptor.Fifties);
                    break;
                case AgeDescriptor.Fifties:
                    toReturn.Add(AgeDescriptor.Forties);
                    toReturn.Add(AgeDescriptor.Sixties);
                    break;
                case AgeDescriptor.Forties:
                    toReturn.Add(AgeDescriptor.Thirties);
                    toReturn.Add(AgeDescriptor.Fifties);
                    break;
                case AgeDescriptor.Thirties:
                    toReturn.Add(AgeDescriptor.Forties);
                    break;
                case AgeDescriptor.Twenties:
                case AgeDescriptor.Teens:
                case AgeDescriptor.PreTeens:
                default:
                    break;
            }

            return toReturn;
        }
    }
}