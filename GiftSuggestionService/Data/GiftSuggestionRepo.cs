using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiftSuggestionService.Models;
using GiftSuggestionService.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GiftSuggestionService.Data
{
    public class GiftSuggestionRepo : IGiftSuggestionRepo
    {
        private readonly IMongoCollection<GiftSuggestion> giftSuggestionsCollection;
        private readonly Dbconfiguration dbConfiguration;

        public GiftSuggestionRepo(
            IOptionsMonitor<Dbconfiguration> optionsMonitor)
        {
            this.dbConfiguration = optionsMonitor.CurrentValue;

            var mongoClient = new MongoClient(
                this.dbConfiguration.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                this.dbConfiguration.DatabaseName);

            this.giftSuggestionsCollection = mongoDatabase.GetCollection<GiftSuggestion>(
                this.dbConfiguration.CollectionName);
        }

        public async Task<List<GiftSuggestion>> GetAsync() =>
            await this.giftSuggestionsCollection.Find(_ => true).ToListAsync();

        public async Task<GiftSuggestion> GetAsync(Guid id) =>
            await this.giftSuggestionsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(GiftSuggestion newGiftSuggestion) =>
            await this.giftSuggestionsCollection.InsertOneAsync(newGiftSuggestion);

        public async Task UpdateAsync(Guid id, GiftSuggestion updatedGiftSuggestion) =>
            await this.giftSuggestionsCollection.ReplaceOneAsync(x => x.Id == id, updatedGiftSuggestion);

        public async Task RemoveAsync(Guid id) =>
            await this.giftSuggestionsCollection.DeleteOneAsync(x => x.Id == id);
    }
}