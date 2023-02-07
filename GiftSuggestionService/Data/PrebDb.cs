using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GiftSuggestionService.Models;
using GiftSuggestionService.Utilities;
using System.Threading.Tasks;

namespace GiftSuggestionService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<IGiftSuggestionRepo>()).Wait();
            }
        }

        private static async Task SeedData(IGiftSuggestionRepo repo)
        {
            foreach (var generatedGiftSuggestion in TestData.TestGeneratedGiftSuggestions)
            {
                await repo.RemoveAsync(GiftSuggestionUtilities.GenerateGiftSuggestionIdFromName(generatedGiftSuggestion.Name));
            }

            if (repo.GetAsync().Result.Count == 0)
            {
                Console.WriteLine("--> Seeding Data...");

                foreach (var giftSuggestion in TestData.TestGiftSuggestions)
                {
                    await repo.CreateAsync(giftSuggestion);
                }
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}