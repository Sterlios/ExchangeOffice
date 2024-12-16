using DatabaseMigrationService.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseMigrationService
{
    internal class ApplicationDbContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCurrency> UserCurrencies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseNpgsql("Host=localhost;Database=CurrencyDb;Username=postgres;Password=0617123");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Currency>().HasKey(currency => currency.Id);
            modelBuilder.Entity<User>().HasKey(user => user.Id);

            modelBuilder.Entity<UserCurrency>()
                .HasKey(userCurrency => new { userCurrency.UserId, userCurrency.CurrencyId });

            modelBuilder.Entity<UserCurrency>()
                .HasOne(userCurrency => userCurrency.User)
                .WithMany(user => user.UserCurrencies)
                .HasForeignKey(userCurrency => userCurrency.UserId);

            modelBuilder.Entity<UserCurrency>()
                .HasOne(userCurrency => userCurrency.Currency)
                .WithMany(currency => currency.UserCurrencies)
                .HasForeignKey(userCurrency => userCurrency.CurrencyId);
        }
    }
}
