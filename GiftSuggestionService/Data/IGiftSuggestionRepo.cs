using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiftSuggestionService.Models;

namespace GiftSuggestionService.Data
{
    public interface IGiftSuggestionRepo
    {
        public Task<List<GiftSuggestion>> GetAsync();

        public Task<GiftSuggestion> GetAsync(Guid id);

        public Task CreateAsync(GiftSuggestion newGiftSuggestion);

        public Task UpdateAsync(Guid id, GiftSuggestion updatedGiftSuggestion);

        public Task RemoveAsync(Guid id);
    }
}