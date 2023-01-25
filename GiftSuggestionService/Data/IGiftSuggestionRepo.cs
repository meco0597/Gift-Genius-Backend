namespace GiftSuggestionService.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GiftSuggestionService.Models;
    using GiftSuggestionService.Dtos;

    public interface IGiftSuggestionRepo
    {
        public Task<List<GiftSuggestion>> GetBySearchParameters(GiftSuggestionSearchDto searchParams);

        public Task<List<GiftSuggestion>> GetAsync();

        public Task<GiftSuggestion> GetAsync(string id);

        public Task CreateAsync(GiftSuggestion newGiftSuggestion);

        public Task UpdateAsync(string id, GiftSuggestion updatedGiftSuggestion);

        public Task RemoveAsync(string id);
    }
}