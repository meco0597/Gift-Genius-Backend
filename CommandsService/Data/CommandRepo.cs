using System;
using System.Collections.Generic;
using System.Linq;
using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateCommand(int giftSuggestionId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.GiftSuggestionId = giftSuggestionId;
            _context.Commands.Add(command);
        }

        public void CreateGiftSuggestion(GiftSuggestion plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }
            _context.GiftSuggestions.Add(plat);
        }

        public bool ExternalGiftSuggestionExists(int externalGiftSuggestionId)
        {
            return _context.GiftSuggestions.Any(p => p.ExternalID == externalGiftSuggestionId);
        }

        public IEnumerable<GiftSuggestion> GetAllGiftSuggestions()
        {
            return _context.GiftSuggestions.ToList();
        }

        public Command GetCommand(int giftSuggestionId, int commandId)
        {
            return _context.Commands
                .Where(c => c.GiftSuggestionId == giftSuggestionId && c.Id == commandId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForGiftSuggestion(int giftSuggestionId)
        {
            return _context.Commands
                .Where(c => c.GiftSuggestionId == giftSuggestionId)
                .OrderBy(c => c.GiftSuggestion.Name);
        }

        public bool PlaformExits(int giftSuggestionId)
        {
            return _context.GiftSuggestions.Any(p => p.Id == giftSuggestionId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}