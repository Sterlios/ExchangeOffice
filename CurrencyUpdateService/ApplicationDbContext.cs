using CurrencyUpdateService.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyUpdateService
{
    internal class ApplicationDbContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.Entity<Currency>().HasKey(currency => currency.Id);
    }
}
