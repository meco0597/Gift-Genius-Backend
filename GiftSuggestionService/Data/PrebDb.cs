using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GiftSuggestionService.Models;

namespace GiftSuggestionService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<IGiftSuggestionRepo>());
            }
        }

        private static void SeedData(IGiftSuggestionRepo repo)
        {
            if (repo.GetAsync().Result.Count == 0)
            {
                Console.WriteLine("--> Seeding Data...");

                repo.CreateAsync(
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
                    });
                repo.CreateAsync(new GiftSuggestion()
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
                });
                repo.CreateAsync(new GiftSuggestion()
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
                });
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}