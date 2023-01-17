using System;
using System.Collections.Generic;
using CommandsService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<GiftSuggestion> giftSuggestions)
        {
            Console.WriteLine("Seeding new giftSuggestions...");

            foreach (var plat in giftSuggestions)
            {
                if (!repo.ExternalGiftSuggestionExists(plat.ExternalID))
                {
                    repo.CreateGiftSuggestion(plat);
                }
                repo.SaveChanges();
            }
        }
    }
}