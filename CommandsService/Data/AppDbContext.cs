using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<GiftSuggestion> GiftSuggestions { get; set; }
        public DbSet<Command> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<GiftSuggestion>()
                .HasMany(p => p.Commands)
                .WithOne(p => p.GiftSuggestion!)
                .HasForeignKey(p => p.GiftSuggestionId);

            modelBuilder
                .Entity<Command>()
                .HasOne(p => p.GiftSuggestion)
                .WithMany(p => p.Commands)
                .HasForeignKey(p => p.GiftSuggestionId);
        }
    }
}