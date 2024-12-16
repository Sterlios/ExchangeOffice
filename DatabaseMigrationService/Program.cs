using Microsoft.EntityFrameworkCore;

namespace DatabaseMigrationService
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Starting migration service...");

            using (var dbContext = new ApplicationDbContext())
            {
                Console.WriteLine("Applying migrations...");
                dbContext.Database.Migrate();
                Console.WriteLine("Migrations applied successfully!");
            }
        }
    }
}
