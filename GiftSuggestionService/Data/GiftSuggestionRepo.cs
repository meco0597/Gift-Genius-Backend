namespace GiftSuggestionService.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GiftSuggestionService.Models;
    using GiftSuggestionService.Configurations;
    using GiftSuggestionService.Services;
    using GiftSuggestionService.Utilities;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using GiftSuggestionService.Dtos;
    using System.Linq.Expressions;

    public class GiftSuggestionRepo : IGiftSuggestionRepo
    {
        private readonly IMongoCollection<GiftSuggestion> giftSuggestionsCollection;
        private readonly Dbconfiguration dbConfiguration;

        public GiftSuggestionRepo(
            IOptionsMonitor<Dbconfiguration> optionsMonitor,
            KeyvaultAccessorService keyvaultAccessor)
        {
            this.dbConfiguration = optionsMonitor.CurrentValue;
            var connectionString = keyvaultAccessor.GetSecretAsync(this.dbConfiguration.KeyvaultSecretName).Result;

            var mongoClient = new MongoClient(connectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                this.dbConfiguration.DatabaseName);

            this.giftSuggestionsCollection = mongoDatabase.GetCollection<GiftSuggestion>(
                this.dbConfiguration.CollectionName);
        }

        public async Task<List<GiftSuggestion>> GetBySearchParameters(GiftSuggestionSearchDto searchParams)
        {
            double pricePadding = 0.3;

            var filterBuilder = Builders<GiftSuggestion>.Filter;
            var filter = filterBuilder.Empty;

            if (searchParams.MinPrice != null && searchParams.MaxPrice != null)
            {
                int minPriceInt = (int)searchParams.MinPrice;
                int maxPriceInt = (int)searchParams.MaxPrice;
                int minPricePadded = minPriceInt.PercentLower(pricePadding);
                int maxPricePadded = maxPriceInt.PercentHigher(pricePadding);
                var priceFilter = filterBuilder.And(
                    filterBuilder.Gt(giftSuggestion => giftSuggestion.MinPrice, minPricePadded),
                    filterBuilder.Lt(giftSuggestion => giftSuggestion.MaxPrice, maxPricePadded));
                filter &= priceFilter;
            }

            if (searchParams.AssociatedInterests != null)
            {
                var interestsFilter = filterBuilder.AnyIn(x => x.AssociatedInterests, searchParams.AssociatedInterests);
                filter &= interestsFilter;
            }

            if (searchParams.AssociatedAge != null)
            {
                var ageRanges = GiftSuggestionUtilities.GetNearbyAges((AgeDescriptor)searchParams.AssociatedAge);
                var ageFilter = filterBuilder.AnyIn(x => x.AssociatedAgeRanges, ageRanges);
                filter &= ageFilter;
            }

            if (searchParams.AssociatedRelationship != null)
            {
                var relationshipFilter = filterBuilder.AnyEq(x => x.AssociatedRelationships, (RelationshipDescriptor)searchParams.AssociatedRelationship);
                filter &= relationshipFilter;
            }

            return await this.giftSuggestionsCollection.Find(filter).ToListAsync();
        }

        public async Task<GiftSuggestion> CreateOrUpdateAsync(GiftSuggestion giftSuggestion)
        {
            string id = GiftSuggestionUtilities.GenerateGiftSuggestionIdFromName(giftSuggestion.GiftName);
            var exisitingGiftSuggestion = await this.GetAsync(id);
            if (exisitingGiftSuggestion == null)
            {
                // create
                GiftSuggestion createdGiftSuggestion = new GiftSuggestion()
                {
                    Id = id,
                    GiftName = giftSuggestion.GiftName,
                    GiftDescription = giftSuggestion.GiftDescription,
                    CreatedAt = DateTime.Now,
                    Link = giftSuggestion.Link,
                    ThumbnailUrl = giftSuggestion.ThumbnailUrl,
                    MinPrice = giftSuggestion.MinPrice,
                    MaxPrice = giftSuggestion.MaxPrice,
                    AssociatedInterests = giftSuggestion.AssociatedInterests,
                    AssociatedAgeRanges = giftSuggestion.AssociatedAgeRanges,
                    AssociatedRelationships = giftSuggestion.AssociatedRelationships,
                    NumOfUpvotes = 0,
                    NumOfClicks = 0,
                    NumOfTimesSuggested = 1,
                };

                await this.CreateAsync(createdGiftSuggestion);
                return createdGiftSuggestion;
            }
            else
            {
                exisitingGiftSuggestion.AssociatedInterests.AddRange(giftSuggestion.AssociatedInterests);
                exisitingGiftSuggestion.AssociatedAgeRanges.AddRange(giftSuggestion.AssociatedAgeRanges);
                exisitingGiftSuggestion.AssociatedRelationships.AddRange(giftSuggestion.AssociatedRelationships);
                // update 
                GiftSuggestion updatedGiftSuggestion = new GiftSuggestion()
                {
                    GiftName = exisitingGiftSuggestion.GiftName,
                    GiftDescription = exisitingGiftSuggestion.GiftDescription,
                    CreatedAt = exisitingGiftSuggestion.CreatedAt,
                    Link = exisitingGiftSuggestion.Link,
                    ThumbnailUrl = exisitingGiftSuggestion.ThumbnailUrl,
                    MinPrice = Math.Min(giftSuggestion.MinPrice, exisitingGiftSuggestion.MinPrice),
                    MaxPrice = Math.Min(giftSuggestion.MaxPrice, exisitingGiftSuggestion.MaxPrice),
                    AssociatedInterests = exisitingGiftSuggestion.AssociatedInterests,
                    AssociatedAgeRanges = exisitingGiftSuggestion.AssociatedAgeRanges,
                    AssociatedRelationships = exisitingGiftSuggestion.AssociatedRelationships,
                    NumOfUpvotes = exisitingGiftSuggestion.NumOfUpvotes,
                    NumOfClicks = exisitingGiftSuggestion.NumOfClicks,
                    NumOfTimesSuggested = exisitingGiftSuggestion.NumOfTimesSuggested + 1,
                };

                await this.UpdateAsync(id, updatedGiftSuggestion);
                return updatedGiftSuggestion;
            }
        }

        public async Task<List<GiftSuggestion>> GetAsync() =>
            await this.giftSuggestionsCollection.Find(_ => true).ToListAsync();

        public async Task<GiftSuggestion> GetAsync(string id) =>
            await this.giftSuggestionsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(GiftSuggestion newGiftSuggestion) =>
            await this.giftSuggestionsCollection.InsertOneAsync(newGiftSuggestion);

        public async Task UpdateAsync(string id, GiftSuggestion updatedGiftSuggestion) =>
            await this.giftSuggestionsCollection.ReplaceOneAsync(x => x.Id == id, updatedGiftSuggestion);

        public async Task RemoveAsync(string id) =>
            await this.giftSuggestionsCollection.DeleteOneAsync(x => x.Id == id);
    }
}