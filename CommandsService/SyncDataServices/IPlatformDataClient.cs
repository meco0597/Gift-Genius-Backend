using System.Collections.Generic;
using CommandsService.Models;

namespace CommandsService.SyncDataServices
{
    public interface IGiftSuggestionDataClient
    {
        IEnumerable<GiftSuggestion> ReturnAllGiftSuggestions();
    }
}