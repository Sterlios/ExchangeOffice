using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CurrencyUpdateService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Добавляем файлы конфигурации (appsettings.json)
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));
                    services.AddDbContext<ApplicationDbContext>();
                    services.AddHostedService<Worker>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}