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
                int normalizedAge = GiftSuggestionUtilities.NormalizeAge((int)searchParams.AssociatedAge);
                var ageFilter = filterBuilder.AnyEq(x => x.AssociatedAgeRanges, normalizedAge);
                filter &= ageFilter;
            }

            if (searchParams.AssociatedOccasion != null)
            {
                var occasionFilter = filterBuilder.AnyEq(x => x.AssociatedOccasions, searchParams.AssociatedOccasion);
                filter &= occasionFilter;
            }

            if (searchParams.AssociatedSex != null)
            {
                var sexFilter = filterBuilder.AnyEq(x => x.AssociatedSex, searchParams.AssociatedSex);
                filter &= sexFilter;
            }

            return await this.giftSuggestionsCollection.Find(filter).ToListAsync();
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