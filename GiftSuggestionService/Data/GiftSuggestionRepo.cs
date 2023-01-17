using System;
using System.Collections.Generic;
using System.Linq;
using GiftSuggestionService.Models;

namespace GiftSuggestionService.Data
{
    public class GiftSuggestionRepo : IGiftSuggestionRepo
    {
        private readonly AppDbContext _context;

        public GiftSuggestionRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateGiftSuggestion(GiftSuggestion plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }

            _context.GiftSuggestions.Add(plat);
        }

        public IEnumerable<GiftSuggestion> GetAllGiftSuggestions()
        {
            return _context.GiftSuggestions.ToList();
        }

        public GiftSuggestion GetGiftSuggestionById(int id)
        {
            return _context.GiftSuggestions.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}