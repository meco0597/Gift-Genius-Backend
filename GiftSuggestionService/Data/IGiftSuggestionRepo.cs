using System.Collections.Generic;
using GiftSuggestionService.Models;

namespace GiftSuggestionService.Data
{
    public interface IGiftSuggestionRepo
    {
        bool SaveChanges();

        IEnumerable<GiftSuggestion> GetAllGiftSuggestions();
        GiftSuggestion GetGiftSuggestionById(int id);
        void CreateGiftSuggestion(GiftSuggestion plat);
    }
}