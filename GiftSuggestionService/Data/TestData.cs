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
                AssociatedInterests = new List<string>() { "golf", "sports" },
                AssociatedRelationships = new List<RelationshipDescriptor>() { RelationshipDescriptor.Father, RelationshipDescriptor.Grandpa, RelationshipDescriptor.Boyfriend, RelationshipDescriptor.Son },
                AssociatedAgeRanges = new List<AgeDescriptor>() { AgeDescriptor.Twenties, AgeDescriptor.Thirties, AgeDescriptor.Sixties },
                NumOfUpvotes = 12_345,
            },
            new GiftSuggestion()
            {
                Id = "surf-wetsuit",
                GiftName = "Surf Wetsuit",
                CreatedAt = System.DateTime.Now,
                MinPrice = 100,
                MaxPrice = 350,
                AssociatedInterests = new List<string>() { "surf", "sports" },
                AssociatedRelationships = new List<RelationshipDescriptor>() { RelationshipDescriptor.Father, RelationshipDescriptor.Friend, RelationshipDescriptor.Boyfriend, RelationshipDescriptor.Son },
                AssociatedAgeRanges = new List<AgeDescriptor>() { AgeDescriptor.Twenties, AgeDescriptor.Forties, AgeDescriptor.Teens },
                NumOfUpvotes = 10_325,
            },
            new GiftSuggestion()
            {
                Id = "scrap-book",
                GiftName = "Scrap Book",
                CreatedAt = System.DateTime.Now,
                MinPrice = 5,
                MaxPrice = 25,
                AssociatedInterests = new List<string>() { "" },
                AssociatedRelationships = new List<RelationshipDescriptor>() { RelationshipDescriptor.Mother, RelationshipDescriptor.Sister, RelationshipDescriptor.Girlfriend },
                AssociatedAgeRanges = new List<AgeDescriptor>() { AgeDescriptor.Twenties, AgeDescriptor.Fifties, AgeDescriptor.Seventies },
                NumOfUpvotes = 122_345,
            },
            new GiftSuggestion()
            {
                Id = "soccer-ball",
                GiftName = "Soccer ball",
                CreatedAt = System.DateTime.Now,
                MinPrice = 15,
                MaxPrice = 25,
                AssociatedInterests = new List<string>() { "soccer" },
                AssociatedRelationships = new List<RelationshipDescriptor>() { RelationshipDescriptor.Father, RelationshipDescriptor.Boyfriend, RelationshipDescriptor.Son },
                AssociatedAgeRanges = new List<AgeDescriptor>() { AgeDescriptor.Twenties, AgeDescriptor.Forties, AgeDescriptor.Teens, AgeDescriptor.MiddleSchool },
                NumOfUpvotes = 1341,
            },
            new GiftSuggestion()
            {
                Id = "skateboard",
                GiftName = "Skateboard",
                CreatedAt = System.DateTime.Now,
                MinPrice = 20,
                MaxPrice = 30,
                AssociatedInterests = new List<string>() { "skateboarding" },
                AssociatedRelationships = new List<RelationshipDescriptor>() { RelationshipDescriptor.Boyfriend, RelationshipDescriptor.Son, RelationshipDescriptor.Brother },
                AssociatedAgeRanges = new List<AgeDescriptor>() { AgeDescriptor.Twenties, AgeDescriptor.Teens, AgeDescriptor.MiddleSchool },
                NumOfUpvotes = 131,
            }
        };

        public static List<GeneratedGiftSuggestion> TestGeneratedGiftSuggestions = new List<GeneratedGiftSuggestion>()
        {
            new GeneratedGiftSuggestion()
            {
                Name = "Cookbook by a celebrity chef",
                Categories = new List<string>() { "cooking" },
            },
            new GeneratedGiftSuggestion()
            {
                Name = "Custom Cutting Board",
                Categories = new List<string>() { "cooking" },
            },
            new GeneratedGiftSuggestion()
            {
                Name = "Songwriting Notebook",
                Categories = new List<string>() { "singing" },
            },
            new GeneratedGiftSuggestion()
            {
                Name = "Poster of a Musical Artist",
                Categories = new List<string>() { "singing" },
            },
            new GeneratedGiftSuggestion()
            {
                Name = "Wine Decanter",
                Categories = new List<string>() { "cooking" },
            }
        };
    }
}