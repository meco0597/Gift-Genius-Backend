using System;
using System.Collections.Generic;
using GiftSuggestionService.Models;

namespace GiftSuggestionService.Data
{
    public static class TestData
    {
        public static List<GiftSuggestion> TestGiftSuggestions = new List<GiftSuggestion>()
        {
            new GiftSuggestion()
            {
                Id = "golf-putter",
                GiftName = "Golf Putter",
                CreatedAt = System.DateTime.Now,
                MinPrice = 80,
                MaxPrice = 250,
                GiftDescription = "They will take 4 strokes off their handicap with a new putter!",
                AssociatedInterests = new List<string>() { "golf", "sports" },
                AssociatedOccasions = new List<string>() { "birthday", "christmas" },
                AssociatedSex = new List<string>() { "Male" },
                AssociatedAgeRanges = new List<int>() { 2, 3, 4, 5, 6, 7 },
                NumOfUpvotes = 12_345,
            },
            new GiftSuggestion()
            {
                Id = "surf-wetsuit",
                GiftName = "Surf Wetsuit",
                CreatedAt = System.DateTime.Now,
                MinPrice = 100,
                MaxPrice = 350,
                GiftDescription = "They will stay warm in the cold waters!",
                AssociatedInterests = new List<string>() { "surf", "sports" },
                AssociatedOccasions = new List<string>() { "birthday", "christmas" },
                AssociatedSex = new List<string>() { "Male" },
                AssociatedAgeRanges = new List<int>() { 2, 3 },
                NumOfUpvotes = 10_325,
            },
            new GiftSuggestion()
            {
                Id = "scrap-book",
                GiftName = "Scrap Book",
                CreatedAt = System.DateTime.Now,
                MinPrice = 5,
                MaxPrice = 25,
                GiftDescription = "They will get to cherish your memories together",
                AssociatedInterests = new List<string>() { "" },
                AssociatedOccasions = new List<string>() { "birthday", "valentines" },
                AssociatedSex = new List<string>() { "Male" },
                AssociatedAgeRanges = new List<int>() { 2, 3, 4, 5, 6, 7 },
                NumOfUpvotes = 122_345,
            },
            new GiftSuggestion()
            {
                Id = "soccer-ball",
                GiftName = "Soccer ball",
                CreatedAt = System.DateTime.Now,
                MinPrice = 15,
                MaxPrice = 25,
                GiftDescription = "A classic gift for any young soccer fan that they can use to practice their skills or play with friends.",
                AssociatedInterests = new List<string>() { "soccer" },
                AssociatedOccasions = new List<string>() { "birthday", "christmas" },
                AssociatedSex = new List<string>() { "Male" },
                AssociatedAgeRanges = new List<int>() { 0, 1, 2, 3 },
                NumOfUpvotes = 1341,
            },
            new GiftSuggestion()
            {
                Id = "skateboard",
                GiftName = "Skateboard",
                CreatedAt = System.DateTime.Now,
                MinPrice = 20,
                MaxPrice = 30,
                GiftDescription = "A perfect gift for the young skateboarding enthusiast to start their adventures on the board.",
                AssociatedInterests = new List<string>() { "skateboarding" },
                AssociatedOccasions = new List<string>() { "birthday", "christmas" },
                AssociatedSex = new List<string>() { "Male" },
                AssociatedAgeRanges = new List<int>() { 0, 1, 2 },
                NumOfUpvotes = 131,
            }
        };

        public static List<GeneratedGiftSuggestion> TestGeneratedGiftSuggestions = new List<GeneratedGiftSuggestion>()
        {
            new GeneratedGiftSuggestion()
            {
                Name = "Cookbook by a celebrity chef",
                Description = "A collection of recipes, instructions, and information about the preparation and serving of foods. This gift will allow them to explore unique cooking recipes.",
                Categories = new List<string>() { "cooking" },
                EstimatedMinPrice = 30,
                EstimatedMaxPrice = 40,
            },
            new GeneratedGiftSuggestion()
            {
                Name = "Custom Cutting Board",
                Description = "A kitchen utensil used as a protective surface on which to cut or slice things. Add a personal touch by customizing it with their name or initials.",
                Categories = new List<string>() { "cooking" },
                EstimatedMinPrice = 20,
                EstimatedMaxPrice = 80,
            },
            new GeneratedGiftSuggestion()
            {
                Name = "Songwriting Notebook",
                Description = "A collection of words, notes, and thoughts from successful songwriters who've had some success themselves, it offers advice on how to create your own masterpiece, and a collection of great ideas to help you get there.",
                Categories = new List<string>() { "singing" },
                EstimatedMinPrice = 10,
                EstimatedMaxPrice = 20,
            },
            new GeneratedGiftSuggestion()
            {
                Name = "Poster of a Musical Artist",
                Description = "A decorative piece that can liven up a space and display personality and musical taste.",
                Categories = new List<string>() { "singing" },
                EstimatedMinPrice = 10,
                EstimatedMaxPrice = 25,
            },
            new GeneratedGiftSuggestion()
            {
                Name = "Wine Decanter",
                Description = "A glass vessel that is used to help aerate wine. For the home cook who serves wine with every meal.",
                Categories = new List<string>() { "cooking" },
                EstimatedMinPrice = 30,
                EstimatedMaxPrice = 50,
            }
        };
    }
}