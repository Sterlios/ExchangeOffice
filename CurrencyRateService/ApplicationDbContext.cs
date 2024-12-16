using CurrencyRateService.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateService
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<UserCurrency> UserCurrencies { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Currency>().HasKey(currency => currency.Id);

            modelBuilder.Entity<UserCurrency>()
                .HasKey(userCurrency => new { userCurrency.UserId, userCurrency.CurrencyId });

            modelBuilder.Entity<UserCurrency>()
                .HasOne(userCurrency => userCurrency.Currency)
                .WithMany(currency => currency.UserCurrencies)
                .HasForeignKey(userCurrency => userCurrency.CurrencyId);
        }
    }
}
