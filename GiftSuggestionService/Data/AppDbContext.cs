using Microsoft.EntityFrameworkCore;
using GiftSuggestionService.Models;

namespace GiftSuggestionService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<GiftSuggestion> GiftSuggestions { get; set; }
    }
}