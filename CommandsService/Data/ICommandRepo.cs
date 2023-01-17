using System.Collections.Generic;
using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        // GiftSuggestions
        IEnumerable<GiftSuggestion> GetAllGiftSuggestions();
        void CreateGiftSuggestion(GiftSuggestion plat);
        bool PlaformExits(int giftSuggestionId);
        bool ExternalGiftSuggestionExists(int externalGiftSuggestionId);

        // Commands
        IEnumerable<Command> GetCommandsForGiftSuggestion(int giftSuggestionId);
        Command GetCommand(int giftSuggestionId, int commandId);
        void CreateCommand(int giftSuggestionId, Command command);
    }
}